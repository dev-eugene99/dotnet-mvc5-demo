﻿using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Following GetFollowing(string followerId, string followeeId)
        {
            return _context.Followings
                .Where(a => a.FollowerId == followerId && a.FolloweeId == followeeId)
                .SingleOrDefault();
        }

        public void AddFollowing(Following following)
        {
            _context.Followings.Add(following);
        }

        public void RemoveFollowing(Following following)
        {
            _context.Followings.Remove(following);
        }
    }
}