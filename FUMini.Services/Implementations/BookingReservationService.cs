using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Interfaces;

namespace FUMini.Services.Implementations
{
    public class BookingReservationService : IBookingReservationService
    {
        private readonly IBookingReservationRepository _bookingReservationRepository;

        public BookingReservationService(IBookingReservationRepository bookingReservationRepository)
        {
            _bookingReservationRepository = bookingReservationRepository;
        }

        public void Add(BookingReservation entity)
        {
            _bookingReservationRepository.Insert(entity);
            _bookingReservationRepository.Save();
        }

        public void Delete(int id)
        {
            _bookingReservationRepository.Delete(id);
            _bookingReservationRepository.Save();
        }

        public void Delete(BookingReservation entity)
        {
            _bookingReservationRepository.Delete(entity);
            _bookingReservationRepository.Save();
        }

        public List<BookingReservation> GetAll()
        {
            return _bookingReservationRepository.GetAll().ToList();
        }

        public BookingReservation GetByID(int id)
        {
            return _bookingReservationRepository.GetByID(id);
        }

        public void Update(BookingReservation entity)
        {
            _bookingReservationRepository.Update(entity);
            _bookingReservationRepository.Save();
        }
    }
}
