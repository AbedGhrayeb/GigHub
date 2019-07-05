using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext context;
        public FollowingsController()
        {
            context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (context.Followings.Any(f=>f.FolloweeId==userId && f.FolloweeId==dto.FolloweeId))
            {
                return BadRequest("Following aready exists");
            }
            var following = new Following
            {
                FollowerId=userId,
                FolloweeId=dto.FolloweeId
            };
            context.Followings.Add(following);
            context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult UnFollow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (context.Followings.Any(f=>f.FolloweeId==userId && f.FolloweeId==dto.FolloweeId))
            {
                var following = context.Followings
               .Where(f => f.FollowerId == userId && f.FolloweeId == dto.FolloweeId)

               .FirstOrDefault();

                context.Followings.Remove(following);
                context.SaveChanges();
            }
            else
            {
                return BadRequest("Following aready exists");

            }

            return Ok();
        }
    }
}
