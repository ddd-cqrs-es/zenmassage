using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NanoMessageBus;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Application
{
    // TODO: We need true async variants of message handler interface
    public class BookingUpdater :
        IMessageHandler<BookingCreatedEvent>,
        IMessageHandler<BookingTenderEvent>,
        IMessageHandler<BookingBidEvent>,
        IMessageHandler<BookingCancelledEvent>,
        IMessageHandler<TherapistBookingAcceptedEvent>,
        IMessageHandler<TherapistBookingConfirmedEvent>
    {
        private readonly IBookingReadRepository _repository;

        public BookingUpdater(IBookingReadRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public async void Handle(BookingCreatedEvent message)
        {
            await _repository
                .AddBooking(
                    message.BookingId,
                    message.ClientId,
                    message.ProposedTime,
                    message.Duration,
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(BookingTenderEvent message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(BookingBidEvent message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(BookingCancelledEvent message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(TherapistBookingAcceptedEvent message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(TherapistBookingConfirmedEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
