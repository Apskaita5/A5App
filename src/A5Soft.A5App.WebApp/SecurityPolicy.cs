using System;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace A5Soft.A5App.WebApp
{
    /// <inheritdoc cref="ISecurityPolicy"/>
    [DefaultServiceImplementation(typeof(ISecurityPolicy))]
    public class SecurityPolicy : ISecurityPolicy
    {
        public SecurityPolicy(IConfiguration config, IWebHostEnvironment environment)
        {
            if (null == config) throw new ArgumentNullException(nameof(config));
            if (null == environment) throw new ArgumentNullException(nameof(environment));

            var usrConfigGroup = nameof(SecurityPolicy);
            if (environment.IsDevelopment()) usrConfigGroup = "Dev" + usrConfigGroup;

            ServerSecret = config[$"{usrConfigGroup}:{nameof(ServerSecret)}"]?.Trim() ?? string.Empty;
            ConfirmPasswordResetMessageSubject = config[$"{nameof(SecurityPolicy)}:{nameof(ConfirmPasswordResetMessageSubject)}"]?.Trim() ?? string.Empty;
            InviteUserMessageSubject = config[$"{nameof(SecurityPolicy)}:{nameof(InviteUserMessageSubject)}"]?.Trim() ?? string.Empty;

            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(TwoFactorTokenLength)}"], out int resultTwoFactorTokenLength))
                TwoFactorTokenLength = resultTwoFactorTokenLength;
            else TwoFactorTokenLength = 6;
            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(TwoFactorAuthenticationTokenExpiresIn)}"], out int resultTwoFactorAuthenticationTokenExpiresIn))
                TwoFactorAuthenticationTokenExpiresIn = resultTwoFactorAuthenticationTokenExpiresIn;
            else TwoFactorAuthenticationTokenExpiresIn = 5;
            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(NewPasswordLength)}"], out int resultNewPasswordLength))
                NewPasswordLength = resultNewPasswordLength;
            else NewPasswordLength = 8;
            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(LockoutAfterFailedAttempts)}"], out int resultLockoutAfterFailedAttempts))
                LockoutAfterFailedAttempts = resultLockoutAfterFailedAttempts;
            else LockoutAfterFailedAttempts = 3;
            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(LockoutDuration)}"], out int resultLockoutDuration))
                LockoutDuration = resultLockoutDuration;
            else LockoutDuration = 20;
            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(ResetPasswordRequestExpiresIn)}"], out int resultResetPasswordRequestExpiresIn))
                ResetPasswordRequestExpiresIn = resultResetPasswordRequestExpiresIn;
            else ResetPasswordRequestExpiresIn = 15;
            if (int.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(IdentityExpiresIn)}"], out int resultIdentityExpiresIn))
                IdentityExpiresIn = resultIdentityExpiresIn;
            else IdentityExpiresIn = 24;

            if (bool.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(RequireTwoFactorAuthentication)}"], out bool resultRequireTwoFactorAuthentication))
                RequireTwoFactorAuthentication = resultRequireTwoFactorAuthentication;
            else RequireTwoFactorAuthentication = false;
            if (bool.TryParse(config[$"{nameof(SecurityPolicy)}:{nameof(CanCreateTenantDatabase)}"], out bool resultCanCreateTenantDatabase))
                CanCreateTenantDatabase = resultCanCreateTenantDatabase;
            else CanCreateTenantDatabase = true;

            if (environment.IsDevelopment())
            {
                ConfirmPasswordResetMessageContent = System.IO.File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "confirmPassReset.html"));
                InviteUserMessageContent = System.IO.File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "inviteUser.html"));
            }
            else
            {
                ConfirmPasswordResetMessageContent = System.IO.File.ReadAllText(System.IO.Path.Combine(environment.ContentRootPath, "confirmPassReset.html"));
                InviteUserMessageContent = System.IO.File.ReadAllText(System.IO.Path.Combine(environment.ContentRootPath, "inviteUser.html"));
            }
        }

        /// <inheritdoc cref="ISecurityPolicy.ServerSecret"/>
        public string ServerSecret { get; }

        /// <inheritdoc cref="ISecurityPolicy.DefaultAuthenticationType"/>
        public string DefaultAuthenticationType => AppAuthenticationHandler.AuthSchemaName;

        /// <inheritdoc cref="ISecurityPolicy.TwoFactorTokenLength"/>
        public int TwoFactorTokenLength { get; }

        /// <inheritdoc cref="ISecurityPolicy.RequireTwoFactorAuthentication"/>
        public bool RequireTwoFactorAuthentication { get; }

        /// <inheritdoc cref="ISecurityPolicy.TwoFactorAuthenticationTokenExpiresIn"/>
        public int TwoFactorAuthenticationTokenExpiresIn { get; }

        /// <inheritdoc cref="ISecurityPolicy.IdentityExpiresIn"/>
        public int IdentityExpiresIn { get; }

        /// <inheritdoc cref="ISecurityPolicy.NewPasswordLength"/>
        public int NewPasswordLength { get; }

        /// <inheritdoc cref="ISecurityPolicy.LockoutAfterFailedAttempts"/>
        public int LockoutAfterFailedAttempts { get; }

        /// <inheritdoc cref="ISecurityPolicy.LockoutDuration"/>
        public int LockoutDuration { get; }

        /// <inheritdoc cref="ISecurityPolicy.ResetPasswordRequestExpiresIn"/>
        public int ResetPasswordRequestExpiresIn { get; }

        /// <inheritdoc cref="ISecurityPolicy.ConfirmPasswordResetMessageSubject"/>
        public string ConfirmPasswordResetMessageSubject { get; }

        /// <inheritdoc cref="ISecurityPolicy.ConfirmPasswordResetMessageContent"/>
        public string ConfirmPasswordResetMessageContent { get; }

        /// <inheritdoc cref="ISecurityPolicy.InviteUserMessageSubject"/>
        public string InviteUserMessageSubject { get; }

        /// <inheritdoc cref="ISecurityPolicy.InviteUserMessageContent"/>
        public string InviteUserMessageContent { get; }

        /// <inheritdoc cref="ISecurityPolicy.ConfirmPasswordResetUrl"/>
        public string ConfirmPasswordResetUrl => @"account\resetpassword?token={0}";

        /// <inheritdoc cref="ISecurityPolicy.CanCreateTenantDatabase"/>
        public bool CanCreateTenantDatabase { get; }
    }
}
