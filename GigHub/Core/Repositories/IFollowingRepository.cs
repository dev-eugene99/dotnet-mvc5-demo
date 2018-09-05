using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string followerId, string followeeId);
        void AddFollowing(Following following);
        void RemoveFollowing(Following following);

    }
}
