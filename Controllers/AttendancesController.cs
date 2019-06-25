using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext context;
        public AttendancesController()
        {
            context = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (context.Attendances.Any(a=>a.AttendeeId==userId && a.GigId == dto.GigId))
               
                return BadRequest("The attendance already exists");
            
            var attendace = new Attendance
            {
                GigId=dto.GigId,
                AttendeeId=User.Identity.GetUserId()
            };
            context.Attendances.Add(attendace);
            context.SaveChanges();
            return Ok();
        }
    }
}
