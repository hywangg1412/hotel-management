using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.UI.ViewModel.Base;
using System.Collections.ObjectModel;

namespace FUMini.UI.ViewModel.User
{
    public class BookingDetailsViewModel : BaseViewModel
    {
        public int BookingReservationID { get; set; }
        public DateOnly? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public ObservableCollection<BookingDetailInfo> BookingDetails { get; set; }

        public BookingDetailsViewModel(BookingReservation booking, IBookingDetailService bookingDetailService, IRoomInformationService roomInformationService)
        {
            BookingReservationID = booking.BookingReservationID;
            BookingDate = booking.BookingDate;
            TotalPrice = booking.TotalPrice;
            BookingDetails = new ObservableCollection<BookingDetailInfo>();

            var details = bookingDetailService.GetAll()
                .Where(bd => bd.BookingReservationID == booking.BookingReservationID)
                .ToList();

            foreach (var detail in details)
            {
                var room = roomInformationService.GetByID(detail.RoomID);
                BookingDetails.Add(new BookingDetailInfo
                {
                    RoomNumber = room?.RoomNumber ?? "N/A",
                    RoomType = room?.RoomType?.RoomTypeName ?? "N/A",
                    StartDate = detail.StartDate,
                    EndDate = detail.EndDate,
                    ActualPrice = detail.ActualPrice,
                    NumberOfDays = (detail.EndDate.ToDateTime(TimeOnly.MinValue) - detail.StartDate.ToDateTime(TimeOnly.MinValue)).Days
                });
            }
        }
    }

    public class BookingDetailInfo
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal? ActualPrice { get; set; }
        public int NumberOfDays { get; set; }
    }
} 