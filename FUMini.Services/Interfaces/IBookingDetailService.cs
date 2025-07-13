using FUMini.BussinessObjects.Models;
using FUMini.Services.Base;

namespace FUMini.Services.Interfaces
{
    public interface IBookingDetailService : IBaseService<BookingDetail, int>
    {
        void UpdateWithRoomChange(int bookingReservationId, int oldRoomId, BookingDetail newDetail);
    }
}
