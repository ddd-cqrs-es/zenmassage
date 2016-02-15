using System;
using System.Runtime.Serialization;

namespace Zen.Massage.Domain.BookingContext
{
    /// <summary>
    /// <c>TherapistBookingId</c> simple object to encapsulate a therapist booking id.
    /// </summary>
    [Serializable]
    [DataContract]
    public struct TherapistBookingId
    {
        /// <summary>
        /// The empty.
        /// </summary>
        public static readonly TherapistBookingId Empty = new TherapistBookingId(Guid.Empty);

        /// <summary>
        /// Initialises a new instance of the <see cref="TherapistBookingId" /> struct.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public TherapistBookingId(Guid id)
            : this()
        {
            Id = id;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid Id { get; private set; }

        /// <summary>
        /// The ==.
        /// </summary>
        /// <param name="lhs">
        /// The lhs.
        /// </param>
        /// <param name="rhs">
        /// The rhs.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator ==(TherapistBookingId lhs, TherapistBookingId rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="lhs">
        /// The lhs.
        /// </param>
        /// <param name="rhs">
        /// The rhs.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator !=(TherapistBookingId lhs, TherapistBookingId rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is TherapistBookingId)
            {
                var rhs = (TherapistBookingId)obj;
                if (rhs.Id == Id)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return string.Format("TherapistBookingId:{0}", Id);
        }
    }
}