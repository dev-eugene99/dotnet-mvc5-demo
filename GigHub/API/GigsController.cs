using GigHub.Interfaces;
using GigHub.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly IGigRepository gigRepository;

        public GigsController()
        {
            gigRepository = new GigRepository();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = await gigRepository.GetGigAsync(id);
            if (gig == null)
                return NotFound();

            if (gig.IsCanceled)
                return NotFound();
            if (gig.ArtistId != userId)
            {
                return StatusCode(System.Net.HttpStatusCode.Forbidden);
            }
            var result = await gigRepository.CancelGigAsync(gig);
            if (result.Item1 == 0)
                return Ok();
            else
                return InternalServerError(new Exception(result.Item2));         
        }
    }
}
