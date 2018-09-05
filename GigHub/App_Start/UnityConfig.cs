using GigHub.Interfaces;
using GigHub.Repositories;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace GigHub
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IGigRepository, GigRepository>();
            container.RegisterType<IArtistRepository, ArtistRepository>();
            container.RegisterType<IGenreRepository, GenreRepository>();
            container.RegisterType<IAttendanceRepository, AttendanceRepository>();
            container.RegisterType<IFollowingRepository, FollowingRepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}