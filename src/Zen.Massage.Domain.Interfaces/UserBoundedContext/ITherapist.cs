using System.Collections.Generic;

namespace Zen.Massage.Domain.UserBoundedContext
{
    public interface ITherapist
    {
        TherapistId TherapistId { get; }

        ICollection<ITherapistSkillEntity> Skills { get; }
    }
}