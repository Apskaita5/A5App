using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a tenant - client company which data is stored in an app database
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_ITenant_DisplayNameForNew),
        NameForOld = nameof(Resources.Security_ITenant_DisplayNameForOld),
        NameForCreateNew = nameof(Resources.Security_ITenant_DisplayNameForCreateNew))]
    public interface ITenant : IDomainEntity
    {

        /// <summary>
        /// A name of the client company which data is stored in an app database.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_ITenant_Name_Description),
            Name = nameof(Resources.Security_ITenant_Name_Name),
            ShortName = nameof(Resources.Security_ITenant_Name_ShortName),
            Prompt = nameof(Resources.Security_ITenant_Name_Prompt))]
        [ValueRequired]
        string Name { get; }

        /// <summary>
        /// A name of the database where the client company data is stored.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_ITenant_DatabaseName_Description),
            Name = nameof(Resources.Security_ITenant_DatabaseName_Name),
            ShortName = nameof(Resources.Security_ITenant_DatabaseName_ShortName),
            Prompt = nameof(Resources.Security_ITenant_DatabaseName_Prompt))]
        [ValueRequired]
        string DatabaseName { get; }

        /// <summary>
        /// Gets an UTC date and time when the entity was created.
        /// Returns null for a new entity not yet saved to the database.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
            Description = nameof(Resources.IAuditableEntity_InsertedAt_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedAt_ShortName),
            GroupName = nameof(Resources.IAuditableEntity_GroupName))]
        DateTime? InsertedAt { get; }

        /// <summary>
        /// Gets an email of the user who created the entity.
        /// Returns string.Empty for a new entity not yet saved to the database.
        /// </summary> 
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
            Description = nameof(Resources.IAuditableEntity_InsertedBy_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedBy_ShortName),
            GroupName = nameof(Resources.IAuditableEntity_GroupName))]
        string InsertedBy { get; }

    }

}
