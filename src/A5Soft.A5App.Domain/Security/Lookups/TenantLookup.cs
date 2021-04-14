using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security.Lookups
{
    /// <summary>
    /// a lookup for <see cref="ITenant"/> reference
    /// </summary>
    [Serializable]
    public class TenantLookup : LookupBase
    {
        protected string _name;
                  
        protected TenantLookup() { }


        /// <inheritdoc cref="ITenant.Name"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ITenant_Name_Description),
            Name = nameof(Resources.Security_ITenant_Name_Name),
            ShortName = nameof(Resources.Security_ITenant_Name_ShortName),
            Prompt = nameof(Resources.Security_ITenant_Name_Prompt))]
        public string Name => _name;


        public override string ToString()
        {
            return _name;
        }

    }
}
