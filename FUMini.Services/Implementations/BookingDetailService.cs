﻿using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Interfaces;
using System.Linq;

namespace FUMini.Services.Implementations
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IBookingDetailRepository _bookingDetailRepository;

        public BookingDetailService(IBookingDetailRepository bookingDetailRepository)
        {
            _bookingDetailRepository = bookingDetailRepository;
        }

        public void Add(BookingDetail entity)
        {
            _bookingDetailRepository.Insert(entity);
            _bookingDetailRepository.Save();
        }

        public void Delete(int id)
        {
            _bookingDetailRepository.Delete(id);
            _bookingDetailRepository.Save();
        }

        public void Delete(BookingDetail entity)
        {
            _bookingDetailRepository.Delete(entity);
            _bookingDetailRepository.Save();
        }

        public List<BookingDetail> GetAll()
        {
            return _bookingDetailRepository.GetAll().ToList();
        }

        public BookingDetail GetByID(int id)
        {
            return _bookingDetailRepository.GetByID(id);
        }

        public void Update(BookingDetail entity)
        {
            _bookingDetailRepository.Update(entity);
            _bookingDetailRepository.Save();
        }

        public void UpdateWithRoomChange(int bookingReservationId, int oldRoomId, BookingDetail newDetail)
        {
            var oldDetail = _bookingDetailRepository.GetAll()
                .FirstOrDefault(bd => bd.BookingReservationID == bookingReservationId && bd.RoomID == oldRoomId);
            if (oldDetail != null)
            {
                _bookingDetailRepository.Delete(oldDetail);
                _bookingDetailRepository.Save();
            }
            _bookingDetailRepository.Insert(newDetail);
            _bookingDetailRepository.Save();
        }
    }
}
