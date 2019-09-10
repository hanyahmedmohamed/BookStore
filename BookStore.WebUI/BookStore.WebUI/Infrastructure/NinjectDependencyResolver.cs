using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.Domain.Concrete;
using System.Configuration;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Infrastructure.Concrete;

namespace BookStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver :IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        


        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            ////WE add bindings here
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(b=>b.Books).Returns(
            //    new List<Book> 
            //    { 
            //        new Book { Title = "SQL Server DB",Price=50M},
            //        new Book { Title = "ASP.NET MVC5",Price=90M},
            //        new Book { Title = "wEB CLIENT",Price=80M}, 
            //    }
            //    );

            EmailSettings emailSettings = new EmailSettings
            {
                //bnsta5dmh lma akon 3ayz a3ml e3dadat 5arg el codes
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IBookRepository>().To < EFBookRepository>();
            //hab3t m3aha el setting bta3t el constractor
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("setting",emailSettings);

            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}