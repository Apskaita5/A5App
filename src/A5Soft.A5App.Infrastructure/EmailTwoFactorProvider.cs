using System;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// <see cref="ITwoFactorProvider"/> implementation using <see cref="IEmailProvider"/>.
    /// </summary>
    [DefaultServiceImplementation(typeof(ITwoFactorProvider))]
    public class EmailTwoFactorProvider : ITwoFactorProvider
    {
        private readonly IEmailProvider _emailProvider;


        public EmailTwoFactorProvider(IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider ?? throw new ArgumentNullException(nameof(emailProvider));
        }


        /// <inheritdoc cref="ITwoFactorProvider.SendSecondFactorRequest"/>
        public async Task SendSecondFactorRequest(string token, UserIdentity user)
        {
            if (token.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(token));
            if (null == user) throw new ArgumentNullException(nameof(user));
            if (user.Email.IsNullOrWhiteSpace()) throw new ArgumentException(
                "User email is not specified.", nameof(user));

            await _emailProvider.SendAsync(new EmailMessage()
            {
                Content = token,
                Email = user.Email,
                Subject = "Authentication code"
            });
        }

    }
}
