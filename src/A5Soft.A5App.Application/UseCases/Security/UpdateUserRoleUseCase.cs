using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IUpdateUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IUpdateUserRoleUseCase))]
    public class UpdateUserRoleUseCase : SaveUseCaseBase<UserRole, IUserRole>, IUpdateUserRoleUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserRoleRepository _repository;


        /// <inheritdoc />
        public UpdateUserRoleUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer,
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, ICacheProvider cache,
            IUserRoleRepository repository) : base(user, authorizer, dataPortal,
            validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<UserRole> DoSaveAsync(IUserRole domainDto)
        {
            if (domainDto.Id.IsNullOrNew()) throw new ValidationException(
                new BrokenRulesCollection(string.Format(Resources.No_Id_For_Updated_Entity,
                    MetadataProvider.GetEntityMetadata<UserRole>().GetDisplayNameForOld())));

            var currentDto = await _repository.FetchAsync(domainDto.Id);

            if (null == currentDto) throw new NotFoundException(typeof(UserRole),
                domainDto.Id.ToString(), MetadataProvider);

            var current = new UserRole(currentDto, ValidationProvider);

            current.Merge(domainDto, true);

            var result = await _repository.UpdateAsync(current.ToDto(), User.Email());

            _cache.Clear(typeof(List<UserRoleLookup>));

            return new UserRole(result, ValidationProvider);
        }

    }
}
