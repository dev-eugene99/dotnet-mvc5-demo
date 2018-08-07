using GigHub.Interfaces;
using GigHub.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly IGigService _gigService;
        public GigsController()
        {
            _gigService = new GigService();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = await _gigService.GetGigByIdAsync(id);
            if (gig != null)
            {
                if (gig.IsCanceled)
                {
                    return NotFound();
                }
                if (gig.ArtistId != userId)
                {
                    return StatusCode(System.Net.HttpStatusCode.Forbidden);
                }
                var result = await _gigService.CancelGigAsync(gig);
                if (result.Item1 == 0)
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError(new Exception(result.Item2));
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
