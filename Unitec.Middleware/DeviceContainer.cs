using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitec.Middleware.Contracts;
using Unitec.Middleware.Devices;

namespace Unitec.Middleware
{
    public static class DeviceContainer
    {
        private static IWindsorContainer container { get; set; }
        static DeviceContainer()
        {
            container = new WindsorContainer();
            container.Register(Component.For<ICreditCardReader>().ImplementedBy<CreditCardReader>()
                       .LifestyleTransient());
        }
        public static T GetDevice<T>()
        {
            return container.Resolve<T>();
        }
        public static void DisposeDevice(IGenericDevice device)
        {
            container.Release(device);
        }
    }
}
