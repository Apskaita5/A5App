using System;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain.Security
{
    [Serializable]
    public class CustomUserRoleIdentity : IDomainEntityIdentity
    {
        public CustomUserRoleIdentity(IDomainEntityIdentity userId, IDomainEntityIdentity tenantId)
        {
            IdentityValue = new IdentityPair(userId, tenantId);
        }


        public IDomainEntityIdentity UserId 
            => ((IdentityPair) IdentityValue)?.UserId;

        public IDomainEntityIdentity TenantId
            => ((IdentityPair)IdentityValue)?.TenantId;

        public Type DomainEntityType 
            => typeof(CustomUserRole);

        public Type IdentityValueType 
            => typeof(CustomUserRoleIdentity);

        public object IdentityValue { get; }

        public bool IsNew 
            => false;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is CustomUserRoleIdentity otherIdentity)
            {
                var userCompareResult = UserId.CompareTo(otherIdentity.UserId);
                if (userCompareResult != 0) return userCompareResult;

                return TenantId.CompareTo(otherIdentity.TenantId);
            }
            throw new ArgumentException("Object is not CustomUserRoleIdentity.");
        }


        [Serializable]
        public class IdentityPair
        {
            public IdentityPair(IDomainEntityIdentity userId, IDomainEntityIdentity tenantId)
            {
                if (userId?.IsNew ?? true) throw new ArgumentNullException(nameof(userId));
                if (tenantId?.IsNew ?? true) throw new ArgumentNullException(nameof(tenantId));
                if (userId.DomainEntityType != typeof(User)) throw new ArgumentException(
                    $"Identity type mismatch ({userId.DomainEntityType.FullName} != Security.User).",
                    nameof(userId));
                if (tenantId.DomainEntityType != typeof(Tenant)) throw new ArgumentException(
                    $"Identity type mismatch ({tenantId.DomainEntityType.FullName} != Security.Tenant).",
                    nameof(tenantId));

                UserId = userId;
                TenantId = tenantId;
            }

            public IDomainEntityIdentity UserId { get; set; }

            public IDomainEntityIdentity TenantId { get; set; }
        }

    }
}
