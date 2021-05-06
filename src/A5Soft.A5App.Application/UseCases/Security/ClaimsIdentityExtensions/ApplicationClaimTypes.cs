namespace A5Soft.A5App.Application.UseCases.Security.ClaimsIdentityExtensions
{
    internal static class ApplicationClaimTypes
    {
        public const string GroupName = "http://schemas.a5soft.com/ws/2021/04/identity/claims/groupname";
        public const string TenantSid = "http://schemas.a5soft.com/ws/2021/04/identity/claims/tenantsid";
        public const string Suspended = "http://schemas.a5soft.com/ws/2021/04/identity/claims/suspended";
        public const string Admin = "http://schemas.a5soft.com/ws/2021/04/identity/claims/admin";
        public const string GroupAdmin = "http://schemas.a5soft.com/ws/2021/04/identity/claims/groupadmin";
        public const string OccHash = "http://schemas.a5soft.com/ws/2021/04/identity/claims/occhash";
        public const string SecurityToken = "http://schemas.a5soft.com/ws/2021/04/identity/claims/securitytoken";
        public const string Permission = "http://schemas.a5soft.com/ws/2021/04/identity/claims/permission/";

        internal const string Namespace = "http://schemas.a5soft.com/ws/";
    }
}
