using Microsoft.Extensions.Configuration;

namespace OnlineEducationPlatform.Shared.Configuration
{
    public class IdentityConfiguration(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
}