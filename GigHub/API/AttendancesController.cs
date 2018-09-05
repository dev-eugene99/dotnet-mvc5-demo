using GigHub.Core;
using GigHub.Core.Models;
using GigHub.DTOs;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{

    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            
            if (_unitOfWork.Attendances.GetAttendance(userId, dto.GigId) != null)
                return BadRequest("The user is already attending this gig.");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _unitOfWork.Attendances.AddAttendance(attendance);

            var response = await _unitOfWork.CompleteAsync();
            if (response.Item1 == 1)
                return InternalServerError();

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> UnAttend(int id)
        {
            var attendance = _unitOfWork.Attendances.GetAttendance(User.Identity.GetUserId(), id);

            if (attendance == null)
                return NotFound();

            _unitOfWork.Attendances.RemoveAttendance(attendance);
            var response = await _unitOfWork.CompleteAsync();
            if (response.Item1 == 1)
                return InternalServerError();

            return Ok();
        }
    }
}
