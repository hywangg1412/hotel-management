using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Base;
using FUMini.DataAccess.Repositories.Interfaces;

namespace FUMini.DataAccess.Repositories.Implementation
{
    public class BookingDetailRepository : GenericRepository<BookingDetail>, IBookingDetailRepository
    {
        private readonly FUMiniContext _context;

        public BookingDetailRepository(FUMiniContext context) : base(context)
        {
            _context = context;
        }
    }
}
