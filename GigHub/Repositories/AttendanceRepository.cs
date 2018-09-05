using GigHub.Interfaces;
using GigHub.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository()
        {
            _context = new ApplicationDbContext();
        }

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Attendance GetAttendance(string userId, int gigId)
        {
            return _context.Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == gigId);
        }

        private delegate Attendance AttendanceDbAction(Attendance attendance);

        public async Task<Tuple<int, string>> AddAttendanceAsync(Attendance attendance)
        {
            return await runDbActionInTryCatch(_context.Attendances.Add, attendance);
        }

        public async Task<Tuple<int, string>> RemoveAttendanceAsync(Attendance attendance)
        {
            return await runDbActionInTryCatch(_context.Attendances.Remove, attendance);
        }

        private async Task<Tuple<int, string>> runDbActionInTryCatch(AttendanceDbAction dbAction, Attendance attendance)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                dbAction(attendance);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }
            return status;
        }
    }
}