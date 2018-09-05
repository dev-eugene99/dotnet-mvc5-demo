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
    public class AttendancesController : ApiController
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendancesController()
        {
            _attendanceRepository = new AttendanceRepository();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            
            if (_attendanceRepository.GetAttendance(userId, dto.GigId) == null)
                return BadRequest("The user is already attending this gig.");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            var response = await _attendanceRepository.AddAttendanceAsync(attendance);

            if (response.Item1 == 1)
                return InternalServerError();

            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> UnAttend(int id)
        {
            var attendance = _attendanceRepository.GetAttendance(User.Identity.GetUserId(), id);

            if (attendance == null)
                return NotFound();

            var response = await _attendanceRepository.RemoveAttendanceAsync(attendance);

            if (response.Item1 == 1)
                return InternalServerError();

            return Ok();
        }
    }
}
