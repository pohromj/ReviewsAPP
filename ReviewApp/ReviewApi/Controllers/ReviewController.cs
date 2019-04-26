using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReviewApi.Models.Database;
using ReviewApi.Models.Review;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ReviewApi.Models.Tameplate;
using ReviewApi.Models.Artifact;

namespace ReviewApi.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Review")]
    public class ReviewController : Controller
    {
        ReviewsDatabaseContext context;
        public ReviewController(ReviewsDatabaseContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("CreateReview")]
        public int CreateReview([FromBody] FullReviewSetup review)
        {
            Review r = new Review()
            {
                Name = review.Setup.Name,
                CloseDate = review.Setup.EndDate,
                Description = review.Setup.Description,
                StartDate = review.Setup.StartDate,
                //WorkproductId = review.Setup.WorkProduct,
                ReviewTameplateId = review.Setup.Tameplate,
                Html = review.Setup.Html               
            };
            var tmp = context.ReviewTameplate.Where(x => x.Id == review.Setup.Tameplate).
                Include(p => p.HeaderRow).FirstOrDefault();
                
            foreach(var t in tmp.HeaderRow)
            {
                HeaderRowData data = new HeaderRowData() { HeaderRowId = t.Id };
                r.HeaderRowData.Add(data);                
            }
            foreach(var p in review.Participant)
            {
                UserReviewRole userReviewRole = new UserReviewRole()
                {
                    UsersEmail = p.Email,
                    ReviewRoleId = p.Role,
                };
                r.UserReviewRole.Add(userReviewRole);
            }
            foreach(int i in review.Artifact)
            {
                var artifact = context.IbmArtifact.Where(a => a.Id == i).FirstOrDefault();
                IbmArtifactReview ar = new IbmArtifactReview();
                ar.IbmArtifactIbmId = artifact.IbmId;
                r.IbmArtifactReview.Add(ar);
                r.IbmArtifact.Add(artifact);

            }
            context.Workproduct.Where(w => w.Id == review.Setup.WorkProduct).FirstOrDefault().Review.Add(r);
            //context.Review.Add(r);
            context.SaveChanges();
            return r.Id;
        }
        [HttpGet]
        [Route("GetReview")]
        public FullReview GetReview(int id)
        {
            FullReview review = new FullReview();
            Review r = context.Review.Where(x => x.Id == id).FirstOrDefault();
            Workproduct w = context.Workproduct.Where(s => s.Id == r.WorkproductId).FirstOrDefault();
            Project p = context.Project.Where(i => i.Id == w.Id).FirstOrDefault();
            Setup setup = new Setup()
            {
                ProjectName = p.Name,
                WorkProductName = w.Name,
                Html = r.Html
            };
            review.Setup = setup;
            review.Participant = GetParticipants(id);
            List<Header> headers = new List<Header>();
            List<HeaderRow> rows = context.HeaderRow.Where(x => x.ReviewTameplateId == r.ReviewTameplateId).ToList();
            for (int i = 0; i < rows.Count; i++)
            {
                string name = context.ReviewColumn.Where(x => x.Id == rows[i].ReviewColumnId).Select(a => a.Name).FirstOrDefault();
                string data = context.HeaderRowData.Where(x => x.ReviewId == r.Id && x.HeaderRowId == rows[i].Id).Select(o => o.Value).FirstOrDefault();
                Header h = new Header() { Fcn = rows[i].Function, Name = rows[i].Name, Id = rows[i].Id, Parameter = rows[i].Parameter, ColumnName = name, Data = data };
                headers.Add(h);
            }
            review.HeadersRow = headers;
            return review;
            
        }
        private List<ParticipantToHeader> GetParticipants(int reviewId)
        {
            List<ParticipantToHeader> participants = new List<ParticipantToHeader>();
            var review = context.Review.Where(r => r.Id == reviewId).Include(role => role.UserReviewRole).FirstOrDefault();
            List<int> rolesId = review.UserReviewRole.Select(x => x.ReviewRoleId).ToList();
            var roles = context.ReviewRole.Where(r => rolesId.Contains(r.Id)).ToList();
            List<string> emails = review.UserReviewRole.Select(x => x.UsersEmail).ToList();
            var userRoles = context.UserReviewRole.Where(u => emails.Contains(u.UsersEmail) &&u.ReviewId == reviewId).ToList();
            var userDetails = context.Users.Where(x => emails.ToList().Contains(x.Email)).ToList();
            foreach(var u in userRoles)
            {
                ParticipantToHeader p = new ParticipantToHeader();
                p.Firstname = userDetails.Where(x => x.Email == u.UsersEmail).FirstOrDefault().Firstname;
                p.Lastname = userDetails.Where(x => x.Email == u.UsersEmail).FirstOrDefault().Lastname;
                p.Role = roles.Where(x => x.Id == u.ReviewRoleId).FirstOrDefault().Name;
                participants.Add(p);
            }
            return participants;       
        }
        [HttpGet]
        [Route("GetMyReviews")]
        public List<ReviewSetup> GetMyReviews()
        {
            List<ReviewSetup> myReviews = new List<ReviewSetup>();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst("UserEmail").Value;
            List<UserReviewRole> review = context.UserReviewRole.Where(x => x.UsersEmail == email).Include(r => r.Review).ToList();
            foreach(var r in review)
            {
                myReviews.Add(new ReviewSetup()
                {
                    Name = r.Review.Name,
                    Id = r.Review.Id
                });
            }
            return myReviews;
        }
        [HttpPost]
        [Route("SaveReviewProgress")]
        public IActionResult SaveReviewProgress([FromBody] ReviewProgress progress)
        {
            Review review = context.Review.Where(x => x.Id == progress.ReviewId).Include(p => p.HeaderRowData).FirstOrDefault();
            review.Html = progress.Html;
            foreach(var p in progress.HeaderDatas)
            {
                var k = review.HeaderRowData.Where(x => x.HeaderRowId == p.HeaderRowId).FirstOrDefault();
                k.Value = p.Data;
            }
            context.SaveChanges();
            return Ok();

        }
        [HttpGet]
        [Route("GetReviewsForArtifact")]
        public ReviewsForArtifact GetReviewsForArtifact(int id)
        {
            ReviewsForArtifact artifact = new ReviewsForArtifact();
            
            var art = context.IbmArtifact.Where(x => x.Id == id).Include(p => p.IbmArtifactReview).FirstOrDefault();
            JazzArtifact a = new JazzArtifact() { Id = art.Id, IbmId = art.IbmId, Name = art.Name, Url = art.Url };
            artifact.Artifact = a;
            List<int>ids = art.IbmArtifactReview.Select(x => x.ReviewId).ToList();
            var reviews = context.Review.Where(x => ids.Contains(x.Id)).ToList();
            List<ReviewSetup> setups = new List<ReviewSetup>();
            foreach(var r in reviews)
            {
                ReviewSetup s = new ReviewSetup() { Id = r.Id, Name = r.Name };
                setups.Add(s);
            }
            artifact.Reviews = setups;
            return artifact;
        }
    }
}