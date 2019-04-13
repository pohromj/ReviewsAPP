using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReviewApi.Models.Database;
using ReviewApi.Models.Review;

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
        public IActionResult CreateReview([FromBody] FullReviewSetup review)
        {
            Review r = new Review()
            {
                Name = review.Setup.Name,
                CloseDate = review.Setup.EndDate,
                Description = review.Setup.Description,
                StartDate = review.Setup.StartDate,
                WorkproductId = review.Setup.WorkProduct,
                ReviewTameplateId = review.Setup.Tameplate
            };

            context.Review.Add(r);
            context.SaveChanges();
            return Ok();
        }
    }
}