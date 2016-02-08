namespace Zen.Massage.Site.Controllers.V1
{
    public enum BookingStatus
    {
        Provisional = 0,
        Tender = 1,
        BidByTherapist = 2,
        AcceptByClient = 3,
        Confirmed = 4,
        CancelledByTherapist = 5,
        CancelledByClient = 6,
        Completed = 7
    }
}