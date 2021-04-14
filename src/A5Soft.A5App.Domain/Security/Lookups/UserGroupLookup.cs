using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security.Lookups
{
    /// <summary>
    /// a lookup for <see cref="IUserGroup"/> reference
    /// </summary>
    [Serializable]
    public class UserGroupLookup : LookupBase
    {
        protected string _groupName;
        protected int _maxUsers;
        protected int _maxTenants;


        protected UserGroupLookup() { }


        /// <inheritdoc cref="IUserGroup.GroupName"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_IUserGroup_GroupName_Description),
            Name = nameof(Resources.Security_IUserGroup_GroupName_Name),
            ShortName = nameof(Resources.Security_IUserGroup_GroupName_ShortName))]
        public string GroupName => _groupName;

        /// <inheritdoc cref="IUserGroup.MaxUsers"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserGroup_MaxUsers_Description),
            Name = nameof(Resources.Security_IUserGroup_MaxUsers_Name),
            ShortName = nameof(Resources.Security_IUserGroup_MaxUsers_ShortName))]
        public int MaxUsers => _maxUsers;

        /// <inheritdoc cref="IUserGroup.MaxTenants"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_IUserGroup_MaxTenants_Description),
            Name = nameof(Resources.Security_IUserGroup_MaxTenants_Name),
            ShortName = nameof(Resources.Security_IUserGroup_MaxTenants_ShortName))]
        public int MaxTenants => _maxTenants;


        public override string ToString()
        {
            return string.Format(Resources.Security_UserGroupLookup_ToString, _groupName);
        }

    }
}
