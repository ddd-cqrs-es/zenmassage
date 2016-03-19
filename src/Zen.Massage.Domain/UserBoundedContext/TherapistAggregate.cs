using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AggregateSource;
using Zen.Massage.Domain.GeneralBoundedContext;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistAggregate : AggregateRootEntity<TherapistState>, ITherapist
    {
        public TherapistAggregate()
        {
            State.Therapist = this;
            State.Applier = ApplyChange;
        }

        public TherapistId TherapistId => State.TherapistId;

        public ICollection<ITherapistSkillEntity> Skills => State.Skills;

        public void Create(TherapistId therapistId)
        {
            if (therapistId == TherapistId.Empty)
            {
                throw new ArgumentException("therapist identifier is invalid", nameof(therapistId));
            }

            ApplyChange(new TherapistCreatedEvent(therapistId));
        }

        public void RegisterSkill(
            TherapyId therapyId, DateTimeOffset acquisitionDate, string supportingInfo)
        {
            // Create skill entry with supporting information
            //  back-office will use this information to verify the requisite skill
            //  
        }
    }
}
