using GigHub.Interfaces;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.Repositories
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserNotificationRepository()
        {
            _context = new ApplicationDbContext();
        }

        public UserNotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}