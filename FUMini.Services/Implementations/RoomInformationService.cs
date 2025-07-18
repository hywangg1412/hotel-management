﻿using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Repositories.Interfaces;
using FUMini.Services.Interfaces;

namespace FUMini.Services.Implementations
{
    public class RoomInformationService : IRoomInformationService
    {
        private readonly IRoomInformationRepository _roomInformationRepository;
        public RoomInformationService(IRoomInformationRepository roomInformationRepository)
        {
            _roomInformationRepository = roomInformationRepository;
        }

        public void Add(RoomInformation entity)
        {
            _roomInformationRepository.Insert(entity);
            _roomInformationRepository.Save();
        }

        public void Delete(int id)
        {
            _roomInformationRepository.Delete(id);
            _roomInformationRepository.Save();
        }

        public void Delete(RoomInformation entity)
        {
            _roomInformationRepository.Delete(entity);
            _roomInformationRepository.Save();
        }

        public List<RoomInformation> GetAll()
        {
            return _roomInformationRepository.GetAll().ToList();
        }

        public RoomInformation GetByID(int id)
        {
            return _roomInformationRepository.GetByID(id);
        }

        public void Update(RoomInformation entity)
        {
            _roomInformationRepository.Update(entity);
            _roomInformationRepository.Save();
        }
    }
}
