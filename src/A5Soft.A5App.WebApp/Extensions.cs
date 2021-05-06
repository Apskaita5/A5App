using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using A5Soft.A5App.Application;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.A5App.Application.UseCases.Security.Tenants;
using A5Soft.A5App.Application.UseCases.Security.UserGroups;
using A5Soft.A5App.Application.UseCases.Security.UserRoles;
using A5Soft.A5App.Application.UseCases.Security.Users;
using A5Soft.A5App.Domain;
using A5Soft.A5App.Infrastructure;
using A5Soft.A5App.Repositories;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Domain;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ServiceLifetime = A5Soft.CARMA.Domain.ServiceLifetime;

namespace A5Soft.A5App.WebApp
{
    public static class Extensions
    {
        #region Configure Services

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddSingleton<ISecurityPolicy>(new SecurityPolicy(configuration, environment));
            services.AddSingleton<IDatabaseConfiguration>(new DatabaseConfiguration(configuration, environment));
            services.AddSingleton<INativeEmailConfiguration>(new NativeEmailConfiguration(configuration, environment));
            services.AddSingleton<A5Soft.CARMA.Application.ILogger, Logger>();
            services.AddSingleton<IBaseUrlProvider, BaseUrlProvider>();
            services.AddScoped<IAuthenticationStateProvider>(sp =>
                (IAuthenticationStateProvider)sp.GetService(typeof(AuthenticationStateProvider)));
            services.AddScoped<SmartNavigationManager>();
            services.AddSingleton<IPluginProvider>(
                new DefaultPluginProvider(new List<(string FolderPath, IPlugin Plugin)>())); //TODO: add plugin engine

            var appServices = RequiredServices.GetBuiltInServices(BuildConfiguration.Server,
                typeof(RequiredServices).Assembly, typeof(IAuditable).Assembly,
                typeof(NativeEmailSender).Assembly, typeof(ISqlDictionaryProvider).Assembly);

            foreach (var service in appServices)
            {
                services.AddApplicationService(service);
            }
        }

        private static void AddApplicationService(this IServiceCollection services, ApplicationServiceInfo applicationService)
        {
            if (applicationService.InjectImplementationOnly) services.AddImplementation(applicationService);
            else if (applicationService.InjectResolver) services.AddTenantResolver(applicationService);
            else services.AddInterfaceAndImplementation(applicationService);
        }

        private static void AddInterfaceAndImplementation(this IServiceCollection services, ApplicationServiceInfo applicationService)
        {
            if (applicationService.Lifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(applicationService.InterfaceType, applicationService.ImplementationType);
            }
            else if (applicationService.Lifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped(applicationService.InterfaceType, applicationService.ImplementationType);
            }
            else
            {
                services.AddTransient(applicationService.InterfaceType, applicationService.ImplementationType);
            }
        }

        private static void AddImplementation(this IServiceCollection services, ApplicationServiceInfo applicationService)
        {
            if (applicationService.Lifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(applicationService.ImplementationType);
            }
            else if (applicationService.Lifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped(applicationService.ImplementationType);
            }
            else
            {
                services.AddTransient(applicationService.ImplementationType);
            }
        }

        private static void AddTenantResolver(this IServiceCollection services, ApplicationServiceInfo applicationService)
        {
            if (applicationService.Lifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(applicationService.InterfaceType, (sp) =>
                {
                    var resolver = (ITenantServiceResolver)sp.GetService(typeof(ITenantServiceResolver));
                    return sp.GetService(resolver.Resolve(applicationService.InterfaceType));
                });
            }
            else if (applicationService.Lifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped(applicationService.InterfaceType, (sp) =>
                {
                    var resolver = (ITenantServiceResolver)sp.GetService(typeof(ITenantServiceResolver));
                    return sp.GetService(resolver.Resolve(applicationService.InterfaceType));
                });
            }
            else
            {
                services.AddTransient(applicationService.InterfaceType, (sp) =>
                {
                    var resolver = (ITenantServiceResolver)sp.GetService(typeof(ITenantServiceResolver));
                    return sp.GetService(resolver.Resolve(applicationService.InterfaceType));
                });
            }
        }

        #endregion
        
        #region ClaimsIdentity Serialization

        /// <summary>
        /// Serializes <see cref="ClaimsIdentity"/> instance.
        /// </summary>
        /// <param name="identity"><see cref="ClaimsIdentity"/> instance to serialize</param>
        /// <returns>serialized string containing identity data</returns>
        /// <remarks><see cref="ClaimsIdentity"/> is not json serializable by itself</remarks>
        public static string Serialize(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var serialized = JsonConvert.SerializeObject(new SerializableClaimsIdentity(identity));

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serialized));
        }

        /// <summary>
        /// Deserializes <see cref="ClaimsIdentity"/> instance that was serialized by <see cref="Serialize"/> method.
        /// </summary>
        /// <param name="serializedIdentity">serialized string containing identity data</param>
        /// <returns><see cref="ClaimsIdentity"/> instance</returns>
        /// <remarks><see cref="ClaimsIdentity"/> is not json serializable by itself</remarks>
        public static ClaimsIdentity DeserializeClaimsIdentity(this string serializedIdentity)
        {
            if (serializedIdentity.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(serializedIdentity));

            var serialized = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(serializedIdentity));

            return JsonConvert.DeserializeObject<SerializableClaimsIdentity>(serialized).ToClaimsIdentity();
        }

        private class SerializableClaim
        {
            public SerializableClaim() { }

            public SerializableClaim(Claim claim)
            {
                Issuer = claim.Issuer;
                Type = claim.Type;
                Value = claim.Value;
                ValueType = claim.ValueType;
            }


            public string Issuer { get; set; }

            public string Type { get; set; }

            public string Value { get; set; }

            public string ValueType { get; set; }


            public Claim ToClaim()
            {
                return new Claim(Type, Value, ValueType, Issuer);
            }

        }

        private class SerializableClaimsIdentity
        {
            public SerializableClaimsIdentity() { }

            public SerializableClaimsIdentity(ClaimsIdentity identity)
            {
                AuthenticationType = identity.AuthenticationType;
                Claims = identity.Claims.Select(c => new SerializableClaim(c)).ToList();
            }


            public string AuthenticationType { get; set; }

            public List<SerializableClaim> Claims { get; set; }


            public ClaimsIdentity ToClaimsIdentity()
            {
                return new ClaimsIdentity(Claims.Select(c => c.ToClaim()), AuthenticationType);
            }

        }

        #endregion
       
        #region Routing For Use Cases

        private static readonly Dictionary<Type, string> _baseUriDictionary = new Dictionary<Type, string>()
        {
            { typeof(IQueryUserGroupsUseCase), "/Security/UserGroups" },
            { typeof(IQueryUserRolesUseCase), "/Security/UserRoles" },
            { typeof(ICreateUserRoleUseCase), "/Security/UserRoleEdit" },
            { typeof(IUpdateUserRoleUseCase), "/Security/UserRoleEdit" },
            { typeof(IChangePasswordUseCase), "/account/changepassword" },
            { typeof(IQueryUsersUseCase), "/Security/Users" },
            { typeof(ICreateUserUseCase), "/Security/UserEdit" },
            { typeof(IUpdateUserUseCase), "/Security/UserEdit" },
            { typeof(IQueryTenantsUseCase), "/" }
        };

        /// <summary>
        /// Gets a route for a component that serves the use case type specified.
        /// </summary>
        /// <param name="useCaseType"></param>
        /// <param name="entityId">an id of the entity to edit (if any)</param>
        public static string RouteForUseCase(this Type useCaseType, string entityId = null)
        {
            if (null == useCaseType) throw new ArgumentNullException(nameof(useCaseType));

            if (_baseUriDictionary.ContainsKey(useCaseType))
            {
                if (entityId.IsNullOrWhiteSpace()) return _baseUriDictionary[useCaseType];
                return _baseUriDictionary[useCaseType] + "/" + entityId.Trim();
            }

            return "/";
        }

        #endregion
    }
}
