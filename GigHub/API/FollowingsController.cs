using GigHub.Core;
using GigHub.Core.Models;
using GigHub.DTOs;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<IHttpActionResult> Follow(UserDto artistDto)
        {
            var userId = User.Identity.GetUserId();

            if (_unitOfWork.Followings.GetFollowing(userId, artistDto.Id) != null)
                return BadRequest("following already exists.");

            var following = new Following
            {
                FolloweeId = artistDto.Id,
                FollowerId = userId
            };

            _unitOfWork.Followings.AddFollowing(following);
            var response = await _unitOfWork.CompleteAsync();
            if (response.Item1 == 1)
                return InternalServerError();



            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow(UserDto artistDto)
        {
            var userId = User.Identity.GetUserId();
            var following = _unitOfWork.Followings.GetFollowing(userId, artistDto.Id);

            if (following == null)
                return NotFound();

            _unitOfWork.Followings.RemoveFollowing(following);
            var response = await _unitOfWork.CompleteAsync();
            if (response.Item1 == 1)
                return InternalServerError();

            return Ok();
        }
    }
}
