using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Zen.Massage.Site
{
    public static class ClaimsHelpers
    {
        private static readonly string AdminClaim = "http://schemas.zenmassage.com/claims/administrator";
        private static readonly string UserIdClaim = "http://schemas.zenmassage.com/claims/userid";
        private static readonly string TherapistClaim = "http://schemas.zenmassage.com/claims/therapist";

        public static Guid GetUserIdClaim(this ClaimsPrincipal principal)
        {
            if (!principal.HasClaim(c => c.Type == UserIdClaim))
            {
                return Guid.Empty;
            }

            // TODO: Validate issuer

            return principal.Claims
                .Where(c => c.Type == UserIdClaim)
                .Select(c => Guid.Parse(c.Value))
                .FirstOrDefault();
        }

        public static bool HasAdminClaim(this ClaimsPrincipal principal)
        {
            // TODO: Validate issuer
            return principal.HasClaim(c => c.Type == AdminClaim);
        }

        public static bool HasTherapistClaim(this ClaimsPrincipal principal)
        {
            // TODO: Validate issuer
            return principal.HasClaim(c => c.Type == TherapistClaim);
        }
    }
}
