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
                HeaderRow headerRow = new HeaderRow() { Function = row.Fcn, Parameter = row.Parameter, Name = row.Name };
                tameplate.HeaderRow.Add(headerRow);
            }
            foreach (string s in model.Role)
            {
                ReviewRole role = new ReviewRole() { Name = s };
                tameplate.ReviewRole.Add(role);
            }
            foreach (ReviewTameplateViewModel m in model.Model)
            {
                ReviewColumn column = new ReviewColumn() { Name = m.ColumnName, Type = m.Type };
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
                var roles = context.ReviewRole.Where(r => r.ReviewTameplateId == id).ToList();
                List<Role> roleList = new List<Role>();
                foreach (var r in roles)
                {
                    Role role = new Role() { Id = r.Id, Name = r.Name };
                    roleList.Add(role);
                }
                tameplate.Roles = roleList;
                var header = context.HeaderRow.Where(h => h.ReviewTameplateId == tmp.Id).ToList();
                List<Header> headers = new List<Header>();
                foreach (var head in header)
                {
                    Header h = new Header() { Fcn = head.Function, Name = head.Name, Parameter = head.Parameter };
                    headers.Add(h);
                }
                tameplate.Header = headers;
                List<Column> columns = new List<Column>();
                var cols = context.ReviewColumn.Where(c => c.ReviewTameplateId == tmp.Id).ToList();
                foreach (var c in cols)
                {
                    Column col = new Column() { ColumnName = c.Name, Type = c.Type };
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
    }
}