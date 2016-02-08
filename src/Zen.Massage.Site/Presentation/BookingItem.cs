using System;

namespace Zen.Massage.Site.Controllers.V1
{
    public class BookingItem
    {
        public Guid EntryId { get; set; }

        public Guid BookingId { get; set; }

        public UserItem Client { get; set; }

        public UserItem Therapist { get; set; }

        public string CustomerName { get; set; }

        public string TherapistName { get; set; }

        public DateTime CreatedDate { get; set; }

        public BookingStatus Status { get; set; }

        public Gender Gender { get; set; }

        public DateTime? StartTime { get; set; }

        public TimeSpan Duration { get; set; }
    }
}