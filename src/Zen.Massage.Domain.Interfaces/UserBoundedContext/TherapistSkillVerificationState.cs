namespace Zen.Massage.Domain.UserBoundedContext
{
    public enum TherapistSkillVerificationState
    {
        Unverified = 0,
        Pending = 1,
        VerifiedValid = 2,
        VerifiedExpired = 3,
        VerifiedRevoked = 4,
    }
}