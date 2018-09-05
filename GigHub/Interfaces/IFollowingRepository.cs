using GigHub.Models;
using System;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string followerId, string followeeId);
        Task<Tuple<int, string>> AddFollowingAsync(Following following);
        Task<Tuple<int, string>> RemoveFollowingAsync(Following following);

    }
}
