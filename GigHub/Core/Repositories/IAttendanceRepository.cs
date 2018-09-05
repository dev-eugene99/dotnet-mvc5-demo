using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IAttendanceRepository
    {        
        Attendance GetAttendance(string userId, int gigId);
        void AddAttendance(Attendance attendance);
        void RemoveAttendance(Attendance attendance);
        
    }
}
