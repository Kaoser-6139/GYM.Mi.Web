using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using GYM.Application.Services;
using GYM.Domain;
using GYM.Domain.Repositories;
using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Infrastructure.Repositories;

namespace GYM.Mi
{
    public class WebModule:Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
               .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>()
               .InstancePerLifetimeScope();



            base.Load(builder);
        }
    }
}
