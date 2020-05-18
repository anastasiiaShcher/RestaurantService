using Autofac;
using Dvor.BLL.Infrastructure;
using Dvor.BLL.Services;
using Dvor.Common.Entities;
using Dvor.Common.Interfaces;
using Dvor.Common.Interfaces.Services;
using Dvor.DAL.Factories;
using Dvor.DAL.Repositories;
using Module = Autofac.Module;

namespace Dvor.Web.Infrastructure.Modules
{
    public class DIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Services

            builder.RegisterType<DishService>().As<IDishService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<ImageService>().As<IService<Image>>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();


            // Repositories

            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Dish>>().As<IRepository<Dish>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<User>>().As<IRepository<User>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Category>>().As<IRepository<Category>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Allergy>>().As<IRepository<Allergy>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Image>>().As<IRepository<Image>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Order>>().As<IRepository<Order>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<OrderDetails>>().As<IRepository<OrderDetails>>().InstancePerLifetimeScope();
            builder.RegisterType<Repository<User>>().As<IRepository<User>>().InstancePerLifetimeScope();


            // Extra

            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<JwtTokenFactory>().As<ITokenFactory>().InstancePerLifetimeScope();
        }
    }
}