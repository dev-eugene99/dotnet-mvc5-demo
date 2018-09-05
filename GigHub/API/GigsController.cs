using GigHub.Core;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = await _unitOfWork.Gigs.GetGigAsync(id);
            if (gig == null)
                return NotFound();

            if (gig.IsCanceled)
                return NotFound();
            if (gig.ArtistId != userId)
            {
                return StatusCode(System.Net.HttpStatusCode.Forbidden);
            }
            _unitOfWork.Gigs.CancelGig(gig);

            var result = await _unitOfWork.CompleteAsync();
            if (result.Item1 == 0)
                return Ok();
            else
                return InternalServerError(new Exception(result.Item2));         
        }
    }
}