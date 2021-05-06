using System;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;

namespace A5Soft.A5App.Application.Permissions.Security
{
    /// <inheritdoc cref="Permission"/>
    public class AccessAuditTrace : Permission
    {
        public override Guid Id { get; } = new Guid("A3141CA5-3886-42A9-9865-BF7ADE974F33");
        public override Guid? PluginId { get; } = null;
        public override int Order { get; } = 0;
        public override bool AllowForSuspendedUser { get; } = false;
        public override string Name => Resources.DefaultPermissions_Security_AccessAuditTrace_Name;
        public override string GroupName => Resources.DefaultPermissions_Security_GroupName_Security;
        public override string ModuleName => Resources.DefaultPermissions_ModuleName_Security;
        public override string Description 
            => Resources.DefaultPermissions_Security_AccessAuditTrace_Description;
    }
}
