using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NanoMessageBus;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

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
                    message.TenantId,
                    message.BookingId,
                    message.CustomerId,
                    message.ProposedTime,
                    message.Duration,
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public async void Handle(BookingTenderEvent message)
        {
            await _repository
                .UpdateBooking(
                    message.TenantId,
                    message.BookingId,
                    BookingStatus.Tender,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public async void Handle(BookingBidEvent message)
        {
            await _repository
                .AddTherapistBooking(
                    message.TenantId,
                    message.BookingId,
                    message.TherapistId,
                    BookingStatus.BidByTherapist,
                    message.ProposedTime,
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public async void Handle(BookingCancelledEvent message)
        {
            if (message.TherapistId != TherapistId.Empty)
            {
                await _repository
                    .UpdateTherapistBooking(
                        message.TenantId,
                        message.BookingId,
                        message.TherapistId,
                        BookingStatus.CancelledByTherapist,
                        null,
                        CancellationToken.None)
                    .ConfigureAwait(false);
            }
            else
            {
                await _repository
                    .UpdateBooking(
                        message.TenantId,
                        message.BookingId,
                        BookingStatus.CancelledByClient,
                        null,
                        null,
                        CancellationToken.None)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public async void Handle(TherapistBookingAcceptedEvent message)
        {
            await _repository
                .UpdateTherapistBooking(
                    message.TenantId,
                    message.BookingId,
                    message.TherapistId,
                    BookingStatus.AcceptByClient,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Handles the message provided.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public async void Handle(TherapistBookingConfirmedEvent message)
        {
            await _repository
                .UpdateTherapistBooking(
                    message.TenantId,
                    message.BookingId,
                    message.TherapistId,
                    BookingStatus.Confirmed,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            await _repository
                .UpdateBooking(
                    message.TenantId,
                    message.BookingId,
                    BookingStatus.Confirmed,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);
        }
    }
}
