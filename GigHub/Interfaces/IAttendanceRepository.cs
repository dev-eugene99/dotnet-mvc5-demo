using GigHub.Models;
using System;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IAttendanceRepository
    {        
        Attendance GetAttendance(string userId, int gigId);
        Task<Tuple<int, string>> AddAttendanceAsync(Attendance attendance);
        Task<Tuple<int, string>> RemoveAttendanceAsync(Attendance attendance);
        
    }
}
