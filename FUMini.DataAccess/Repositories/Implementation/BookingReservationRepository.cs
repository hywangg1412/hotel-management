using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Base;
using FUMini.DataAccess.Repositories.Interfaces;

namespace FUMini.DataAccess.Repositories.Implementation
{
    public class BookingReservationRepository : GenericRepository<BookingReservation>, IBookingReservationRepository
    {
        private readonly FUMiniContext _context;
        public BookingReservationRepository(FUMiniContext context) : base(context)
        {
            _context = context;
        }
    }
}
