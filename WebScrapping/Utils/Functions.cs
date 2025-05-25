using Microsoft.IdentityModel.Tokens;

namespace WebScrapping.Utils
{
    public static class Functions
    {
        public static bool ValidateToken(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            bool valid = false;
            if (expires.HasValue && DateTime.UtcNow < expires) valid = true;
            return valid;

        }
    }
}
