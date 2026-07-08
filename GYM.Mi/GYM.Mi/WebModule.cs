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

            //Users

            builder.RegisterType<UserRepository>().As<IUserRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<AIService>().As<IAiService>()
                .InstancePerLifetimeScope();

            //Equipments

            builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EquipmentService>().As<IEquipmentService>()
                .InstancePerLifetimeScope();

            //Employee
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>()
               .InstancePerLifetimeScope();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>()
                .InstancePerLifetimeScope();

            // Membership
            builder.RegisterType<MembershipRepository>().As<IMembershipRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MembershipService>().As<IMembershipService>()
                .InstancePerLifetimeScope();

            //Blog
            builder.RegisterType<BlogRepository>().As<IBlogRepository>()
                  .InstancePerLifetimeScope();

            builder.RegisterType<BlogService>().As<IBlogService>()
                .InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
