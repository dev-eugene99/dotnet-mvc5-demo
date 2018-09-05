using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using System.Web.Http;
using System.Web.Mvc;
using Unity;

namespace GigHub
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IArtistRepository, ArtistRepository>();
            container.RegisterType<IAttendanceRepository, AttendanceRepository>();
            container.RegisterType<IFollowingRepository, FollowingRepository>();
            container.RegisterType<IGenreRepository, GenreRepository>();
            container.RegisterType<IGigRepository, GigRepository>();
            container.RegisterType<IUserNotificationRepository, UserNotificationRepository>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}
