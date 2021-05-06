using A5Soft.A5App.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.WebApp
{
    /// <inheritdoc cref="INativeEmailConfiguration"/>
    [DefaultServiceImplementation(typeof(INativeEmailConfiguration))]
    public class NativeEmailConfiguration : INativeEmailConfiguration
    {
        public NativeEmailConfiguration(IConfiguration config, IWebHostEnvironment environment)
        {
            if (null == config) throw new ArgumentNullException(nameof(config));
            if (null == environment) throw new ArgumentNullException(nameof(environment));

            var usrConfigGroup = nameof(NativeEmailConfiguration);
            if (environment.IsDevelopment()) usrConfigGroup = "Dev" + usrConfigGroup;

            EmailAddress = config[$"{usrConfigGroup}:{nameof(EmailAddress)}"]?.Trim() ?? string.Empty;
            DisplayName = config[$"{nameof(NativeEmailConfiguration)}:{nameof(DisplayName)}"]?.Trim() ?? string.Empty;
            Host = config[$"{usrConfigGroup}:{nameof(Host)}"]?.Trim() ?? string.Empty;
            UserName = config[$"{usrConfigGroup}:{nameof(UserName)}"]?.Trim() ?? string.Empty;
            Password = config[$"{usrConfigGroup}:{nameof(Password)}"]?.Trim() ?? string.Empty;

            if (int.TryParse(config[$"{usrConfigGroup}:{nameof(Port)}"], out int resultPort))
                Port = resultPort;
            else Port = 0;
            if (int.TryParse(config[$"{nameof(NativeEmailConfiguration)}:{nameof(Retries)}"], out int resultRetries))
                Retries = resultRetries;
            else Retries = 3;
            if (int.TryParse(config[$"{nameof(NativeEmailConfiguration)}:{nameof(RetryWaitPeriod)}"], 
                out int resultRetryWaitPeriod)) RetryWaitPeriod = resultRetryWaitPeriod;
            else RetryWaitPeriod = 5000;

            if (bool.TryParse(config[$"{usrConfigGroup}:{nameof(EnableSsl)}"], 
                out bool resultEnableSsl)) EnableSsl = resultEnableSsl;
            else EnableSsl = false;
            if (bool.TryParse(config[$"{usrConfigGroup}:{nameof(UseDefaultCredentials)}"], 
                out bool resultUseDefaultCredentials)) UseDefaultCredentials = resultUseDefaultCredentials;
            else UseDefaultCredentials = true;
        }

        /// <inheritdoc cref="INativeEmailConfiguration.EmailAddress"/>
        public string EmailAddress { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.DisplayName"/>
        public string DisplayName { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.Host"/>
        public string Host { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.Port"/>
        public int Port { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.EnableSsl"/>
        public bool EnableSsl { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.UseDefaultCredentials"/>
        public bool UseDefaultCredentials { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.UserName"/>
        public string UserName { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.Password"/>
        public string Password { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.Retries"/>
        public int Retries { get; }

        /// <inheritdoc cref="INativeEmailConfiguration.RetryWaitPeriod"/>
        public int RetryWaitPeriod { get; }
    }
}
