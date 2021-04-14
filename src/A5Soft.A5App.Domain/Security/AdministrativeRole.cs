using A5Soft.A5App.Domain.Properties;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// administrative privilege type that can be assigned for a user
    /// </summary>
    public enum AdministrativeRole
    {
        [Display(ResourceType = typeof(Resources), 
            Description = nameof(Resources.Security_AdministrativeRole_Admin_Description),
            Name = nameof(Resources.Security_AdministrativeRole_Admin_Name),
            ShortName = nameof(Resources.Security_AdministrativeRole_Admin_ShortName))]
        Admin = 2,

        [Display(ResourceType = typeof(Resources), 
            Description = nameof(Resources.Security_AdministrativeRole_GroupAdmin_Description),
            Name = nameof(Resources.Security_AdministrativeRole_GroupAdmin_Name),
            ShortName = nameof(Resources.Security_AdministrativeRole_GroupAdmin_ShortName))]
        GroupAdmin = 1,

        [Display(ResourceType = typeof(Resources), 
            Description = nameof(Resources.Security_AdministrativeRole_None_Description),
            Name = nameof(Resources.Security_AdministrativeRole_None_Name),
            ShortName = nameof(Resources.Security_AdministrativeRole_None_ShortName))]
        None = 0
    }
}
