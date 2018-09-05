using GigHub.DTOs;
using GigHub.Interfaces;
using GigHub.Models;
using GigHub.Repositories;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IFollowingRepository _followingRepository;

        public FollowingsController()
        {
            _followingRepository = new FollowingRepository();
        }

        public async Task<IHttpActionResult> Follow(UserDto artistDto)
        {
            var userId = User.Identity.GetUserId();

            if (_followingRepository.GetFollowing(userId, artistDto.Id) != null)
                return BadRequest("following already exists.");

            var following = new Following
            {
                FolloweeId = artistDto.Id,
                FollowerId = userId
            };

            var response = await _followingRepository.AddFollowingAsync(following);
            if (response.Item1 == 1)
                return InternalServerError();



            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow(UserDto artistDto)
        {
            var userId = User.Identity.GetUserId();
            var following = _followingRepository.GetFollowing(userId, artistDto.Id);

            if (following == null)
                return NotFound();

            var response = await _followingRepository.RemoveFollowingAsync(following);
            if (response.Item1 == 1)
                return InternalServerError();

            return Ok();
        }
    }
}
