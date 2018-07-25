﻿using GigHub.DTOs;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult Follow(ArtistDto artistDto)
        {
            var userId = User.Identity.GetUserId();

            if (_context.Followings.Any(a => a.FollowerId == userId && a.FolloweeId == artistDto.ArtistId))
                return BadRequest("following already exists.");

            var following = new Following
            {
                FolloweeId = artistDto.ArtistId,
                FollowerId = userId
            };
            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }
    }
}
