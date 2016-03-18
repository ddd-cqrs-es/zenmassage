using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Application
{
    public class UserManagementService
    {
        public Task<CustomerId> SignupCustomer(
            TenantId tenantId,
            string firstName,
            string lstName,
            string nickName,
            string emailAddress)
        {
            // TODO: Use ADAL to get or add user via Active Directory

            // TODO: Armed with user, associate with groups
            // NOTE: We will create group names that incorporate the tenant id
            //  so the same user can have single-sign-on with multiple tenants
            throw new NotImplementedException();
        }

        public Task<TherapistId> SignupTherapist(
            TenantId tenantId,
            string firstName,
            string lstName,
            string nickName,
            string emailAddress)
        {
            // TODO: Use ADAL to get or add user via Active Directory

            // Setup groups
            // NOTE:
            //  1. Therapist accreditation is a manual process managed by the tenant
            //      Each skill added by a therapist would require separate verification
            //      once a skill has been accredited then the therapist claim would be
            //      added to the account for this tenant.
            //  2. Therapist skills are global (ie: not tenant specific)
            //  3. Veriication claims of therapist skills ARE tenant specific
            throw new NotImplementedException();
        }

        public Task<TenantId> SignupTenant(
            string firstName,
            string lastName,
            string emailAddress,
            string tenantName,
            string tenantSlug)
        {
            throw new NotImplementedException();
        }
    }
}
