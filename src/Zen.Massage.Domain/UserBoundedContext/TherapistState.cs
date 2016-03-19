using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AggregateSource;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public class TherapistState : EntityState
    {
        private readonly IList<TherapistSkillEntity> _skills =
            new List<TherapistSkillEntity>();
         
        public TherapistState()
        {
            // Register event handlers
            Register<TherapistCreatedEvent>(OnTherapistCreated);
        }

        public TherapistAggregate Therapist { get; set; }

        public Action<object> Applier { get; set; }

        public TherapistId TherapistId { get; private set; }

        public IClientBasicInformation BasicInformation { get; private set; }

        public ICollection<ITherapistSkillEntity> Skills =>
            new ReadOnlyCollection<ITherapistSkillEntity>(
                _skills.Cast<ITherapistSkillEntity>().ToList());

        private void OnTherapistCreated(TherapistCreatedEvent eventObject)
        {
            TherapistId = eventObject.TherapistId;
        }
    }
}