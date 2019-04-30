using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReviewApi.Models.Database;
using ReviewApi.Models.Tameplate;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ReviewApi.Controllers
{
    [Authorize]
    
    [Route("api/Tameplate")]
    public class TameplateController : Controller
    {
        ReviewsDatabaseContext context;
        public TameplateController(ReviewsDatabaseContext context)
        {
            this.context = context;
        }
        [Route("CreateTameplate")]
        public IActionResult CreateTameplate([FromBody] ReviewTameplateModel model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst("UserEmail").Value;
            ReviewTameplate tameplate = new ReviewTameplate() { Name = model.Name, Description = model.Descritpion, UsersEmail = email };
            foreach (var row in model.Header)
            {
                if (row.ColumnName == null)
                {
                    HeaderRow headerRow = new HeaderRow() { Function = row.Fcn, Parameter = row.Parameter, Name = row.Name };
                    tameplate.HeaderRow.Add(headerRow);
                }
            }
            foreach (string s in model.Role)
            {
                ReviewRole role = new ReviewRole() { Name = s };
                tameplate.ReviewRole.Add(role);
            }
            foreach (ReviewTameplateViewModel m in model.Model)
            {
                ReviewColumn column = new ReviewColumn() { Name = m.ColumnName, Type = m.Type };
                
                ReviewHeader row = model.Header.Where(x => x.ColumnName == m.ColumnName).FirstOrDefault();
                if(row != null)
                {
                    HeaderRow headerRow = new HeaderRow() { Function = row.Fcn, Parameter = row.Parameter, Name = row.Name };
                    column.HeaderRow.Add(headerRow);
                    tameplate.HeaderRow.Add(headerRow);
                    //context.HeaderRow.Add(headerRow);
                    //headerRow.ReviewColumn = column;
                }
                tameplate.ReviewColumn.Add(column);
                if (m.Option != null)
                    foreach (string s in m.Option)
                    {
                        ReviewColumnTypeEnum en = new ReviewColumnTypeEnum() { Name = s };
                        column.ReviewColumnTypeEnum.Add(en);
                    }

            }
            context.ReviewTameplate.Add(tameplate);
            context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("GetAllTameplates")]
        public IEnumerable<ReviewTamplateNames> GetAllTameplates()
        {
            List<ReviewTamplateNames> names = new List<ReviewTamplateNames>();
            foreach (var tmp in context.ReviewTameplate)
            {
                names.Add(new ReviewTamplateNames() { ID = tmp.Id, Name = tmp.Name });
            }
            return names;
        }

        [HttpGet]
        [Route("GetTameplate")]
        public ReviewTameplateForForm GetTameplate(int id)
        {
            ReviewTameplateForForm tameplate = new ReviewTameplateForForm();
            var tmp = context.ReviewTameplate.Where(t => t.Id == id).FirstOrDefault();
            if (tmp != null)
            {
                tameplate.Id = tmp.Id;
                tameplate.Name = tmp.Name;
                tameplate.Descritpion = tmp.Description;
                var roles = context.ReviewRole.Where(r => r.ReviewTameplateId == id && r.Deleted != true).ToList();
                List<Role> roleList = new List<Role>();
                foreach (var r in roles)
                {
                    Role role = new Role() { Id = r.Id, Name = r.Name };
                    roleList.Add(role);
                }
                tameplate.Roles = roleList;
                var header = context.HeaderRow.Where(h => h.ReviewTameplateId == tmp.Id && h.Deleted != true).ToList();
                List<Header> headers = new List<Header>();
                foreach (var head in header)
                {
                    if (head.ReviewColumnId != null)
                    {
                        ReviewColumn c = context.ReviewColumn.Where(x => x.Id == head.ReviewColumnId).FirstOrDefault();
                        Header h = new Header() { Fcn = head.Function, Name = head.Name, Parameter = head.Parameter, ColumnName = c.Name, Id = head.Id, ColumnId = head.ReviewColumnId };
                        headers.Add(h);
                        
                    }
                    else
                    {
                        Header h = new Header() { Fcn = head.Function, Name = head.Name, Parameter = head.Parameter, Id = head.Id };
                        headers.Add(h);
                    }
                }
                tameplate.Header = headers;
                List<Column> columns = new List<Column>();
                var cols = context.ReviewColumn.Where(c => c.ReviewTameplateId == tmp.Id && c.Deleted != true).ToList();
                foreach (var c in cols)
                {
                    Column col = new Column() { ColumnName = c.Name, Type = c.Type, Id = c.Id };
                    var opt = context.ReviewColumnTypeEnum.Where(o => o.ReviewColumnId == c.Id).ToList();
                    List<string> options = new List<string>();
                    foreach (var o in opt)
                    {
                        options.Add(o.Name);
                    }
                    col.Option = options;
                    columns.Add(col);
                }
                tameplate.Columns = columns;
                return tameplate;
            }
            return null;

        }
        [HttpPut]
        [Route("UpdateTemplate")]
        public IActionResult UpdateTemplate([FromBody] ReviewTameplateForForm model)
        {
            var review = context.ReviewTameplate.Where(x => x.Id == model.Id).Include(x => x.ReviewColumn).Include(x => x.HeaderRow).Include(x =>x.ReviewRole).FirstOrDefault();
            review.ReviewColumn = review.ReviewColumn.Select(x => { x.Deleted = true; return x; }).ToList();
            review.HeaderRow = review.HeaderRow.Select(x => { x.Deleted = true; return x; }).ToList();
            review.ReviewRole = review.ReviewRole.Select(x => { x.Deleted = true; return x; }).ToList();
            review.Name = model.Name;
            review.Description = model.Descritpion;

            foreach(var r in model.Roles)
            {
                ReviewRole role = review.ReviewRole.Where(x => x.Id == r.Id).FirstOrDefault();
                if(role != null)
                {
                    role.Name = r.Name;
                    role.Deleted = false;
                }
                else
                {
                    ReviewRole rl = new ReviewRole() { Name = r.Name };
                    review.ReviewRole.Add(rl);
                }
            }


            foreach (var c in model.Columns)
            {
                ReviewColumn column = review.ReviewColumn.Where(x => x.Id == c.Id).FirstOrDefault();
                if(column != null)
                {
                    var enums = context.ReviewColumnTypeEnum.Where(x => x.ReviewColumnId == column.Id).ToList();
                    context.ReviewColumnTypeEnum.RemoveRange(enums);//delete all enum and create new
                    column.Type = c.Type;
                    column.Name = c.ColumnName;
                    column.Deleted = false;
                    //var isHeaderMakeSense = context.ReviewColumn.Where(x => x.Id == column.Id).Include(x => x.HeaderRow).FirstOrDefault();
                    //bool sense = isHeaderMakeSense.HeaderRow.
                    if(enums != null &&  c.Option != null)
                        foreach(string s in c.Option)
                        {
                            ReviewColumnTypeEnum t = new ReviewColumnTypeEnum() { Name = s };
                            column.ReviewColumnTypeEnum.Add(t);
                        }
                }
                else
                {
                    ReviewColumn cl = new ReviewColumn() { Name = c.ColumnName, Type = c.Type };
                    if(c.Option != null)
                        foreach(string s in c.Option)
                        {
                            ReviewColumnTypeEnum t = new ReviewColumnTypeEnum() { Name = s };
                            cl.ReviewColumnTypeEnum.Add(t);
                        }
                    review.ReviewColumn.Add(cl);
                }
            }
            foreach(var h in model.Header)
            {
                if (h.Id != null)
                {
                    review.HeaderRow.Where(x => x.Id == h.Id).FirstOrDefault().Deleted = false;
                }
                else
                {
                    var col = review.ReviewColumn.Where(x => x.Name == h.ColumnName).FirstOrDefault();
                    if (col != null)
                    {
                        HeaderRow header = new HeaderRow() { Function = h.Fcn, ReviewColumnId = col.Id, Name = h.Name, Parameter = h.Parameter, ReviewTameplateId = review.Id };
                        context.HeaderRow.Add(header);
                    }
                    else
                    {
                        HeaderRow header = new HeaderRow() {  Name = h.Name, ReviewTameplateId = review.Id };
                        context.HeaderRow.Add(header);
                    }
                }
            }

            context.SaveChanges();
            return Ok();

        }
        [HttpDelete]
        [Route("DeleteTemplate")]
        public IActionResult DeleteTemplate(int id)
        {
            return Ok();
        }
    }
}