using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext context;
        public GigsController()
        {
            context = new ApplicationDbContext();
        }
        [HttpDelete]
        public IHttpActionResult Canceled(int id)
        {
            var userId = User.Identity.GetUserId();
            var canceledGig = context.Gigs
                .Include(g=>g.Attendances.Select(a=>a.Attendee))
                .Single(g => g.Id == id && g.ArtistId == userId);
            if (canceledGig.IsCanceled)
            {
                return NotFound();
            }

            canceledGig.Cancel();
        
            context.SaveChanges();
            return Ok();
        }
      

    }
}
