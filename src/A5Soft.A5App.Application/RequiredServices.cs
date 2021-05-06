using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using A5Soft.A5App.Domain;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application
{
    public static class RequiredServices
    {
        public static IEnumerable<ApplicationServiceInfo> GetBuiltInServices(BuildConfiguration buildConfiguration,
            params Assembly[] applicationAssemblies)
        {
            var result = IoC.GetDefaultServices(buildConfiguration, applicationAssemblies);

            var frameworkServices = IoC.GetFrameworkServices()
                .Where(s => !result
                    .Any(r => s.InterfaceType == r.InterfaceType))
                .ToList();

            result.AddRange(frameworkServices);

            return result.Select(s => new ApplicationServiceInfo(s));
        }

       

    }
}
