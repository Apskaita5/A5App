using A5Soft.A5App.Application.Properties;
using A5Soft.CARMA.Application.Navigation;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A main menu of the app.
    /// </summary>
    public static class ApplicationMenu 
    {
        /// <summary>
        /// A name of the main menu group for common (administrative) tasks.
        /// </summary>
        public const string CommonMenuGroupName = "CommonGroup";

        /// <summary>
        /// A name of the main menu group for user administration tasks.
        /// </summary>
        public const string UserAdminMenuGroupName = "UserAdminGroup";

        /// <summary>
        /// A name of the main menu group for tenant login (contains a list of <see cref="LoginTenantMenuItem"/>).
        /// </summary>
        internal const string LoginTenantMenuGroupName = "LoginTenantMenuGroup";


        /// <summary>
        /// Gets a builtin main menu for the application. 
        /// </summary>
        internal static MainMenu GetMainMenu()
        {
            var result = new MainMenu();

            var commonMenuGroup = result.AddMainMenuTopGroup(CommonMenuGroupName,
                nameof(Properties.Resources.ApplicationMenu_CommonGroup_Name),
                 nameof(Properties.Resources.ApplicationMenu_CommonGroup_Description), typeof(Resources));

            var userAdminMenuGroup = commonMenuGroup.AddMenuGroup(UserAdminMenuGroupName,
                nameof(Properties.Resources.ApplicationMenu_UserAdminMenuGroup_Name),
                nameof(Properties.Resources.ApplicationMenu_UserAdminMenuGroup_Description), typeof(Resources));
            userAdminMenuGroup.AddLeaf("usersMenuItem",
                nameof(Properties.Resources.ApplicationMenu_UsersMenuItem_Name),
                nameof(Properties.Resources.ApplicationMenu_UsersMenuItem_Description),
                typeof(Resources), typeof(UseCases.Security.Users.IQueryUsersUseCase));
            userAdminMenuGroup.AddLeaf("userRolesMenuItem",
                nameof(Properties.Resources.ApplicationMenu_UserRolesMenuItem_Name),
                nameof(Properties.Resources.ApplicationMenu_UserRolesMenuItem_Description),
                typeof(Resources), typeof(UseCases.Security.UserRoles.IQueryUserRolesUseCase));
            userAdminMenuGroup.AddLeaf("userGroupsMenuItem",
                nameof(Properties.Resources.ApplicationMenu_UserGroupsMenuItem_Name),
                nameof(Properties.Resources.ApplicationMenu_UserGroupsMenuItem_Description),
                typeof(Resources), typeof(UseCases.Security.UserGroups.IQueryUserGroupsUseCase));

            var loginTenantMenuGroup = commonMenuGroup.AddMenuGroup(LoginTenantMenuGroupName,
                nameof(Properties.Resources.ApplicationMenu_LoginTenantMenuGroup_Name),
                nameof(Properties.Resources.ApplicationMenu_LoginTenantMenuGroup_Description), typeof(Resources));

            commonMenuGroup.Items.Add(new LoginTenantMenuItem(null)); // logout 

            commonMenuGroup.AddLeaf("changePasswordMenuItem",
                nameof(Properties.Resources.ApplicationMenu_ChangePasswordMenuItem_Name),
                nameof(Properties.Resources.ApplicationMenu_ChangePasswordMenuItem_Description),
                typeof(Resources), typeof(IChangePasswordUseCase));

            return result;
        }

    }
}
