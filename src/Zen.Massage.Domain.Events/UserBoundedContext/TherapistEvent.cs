using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zen.Massage.Domain.UserBoundedContext
{
    [Serializable]
    public class TherapistEvent
    {
        public TherapistEvent(TherapistId therapistId)
        {
            TherapistId = therapistId;
        }

        public TherapistId TherapistId { get; private set; }
    }
}
