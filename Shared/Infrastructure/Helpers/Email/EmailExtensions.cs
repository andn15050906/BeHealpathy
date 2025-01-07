using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Helpers.Email;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services, EmailOptions emailOptions)
    {
        services.Configure<EmailOptions>(options =>
        {
            options.SenderMail = emailOptions.SenderMail;
            options.SenderPassword = emailOptions.SenderPassword;
        });
        services.AddScoped<EmailService>();
        return services;
    }
}
