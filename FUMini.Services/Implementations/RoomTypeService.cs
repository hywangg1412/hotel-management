using FUMini.BussinessObjects.Models;
using FUMini.Services.Interfaces;
using FUMini.DataAccess.Repositories.Interfaces;

namespace FUMini.Services.Implementations
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public void Add(RoomType entity)
        {
            _roomTypeRepository.Insert(entity);
            _roomTypeRepository.Save();
        }

        public void Delete(int id)
        {
            _roomTypeRepository.Delete(id);
            _roomTypeRepository.Save();
        }

        public void Delete(RoomType entity)
        {
            _roomTypeRepository.Delete(entity);
            _roomTypeRepository.Save();
        }

        public IEnumerable<RoomType> GetAll()
        {
            return _roomTypeRepository.GetAll();
        }

        public RoomType GetByID(int id)
        {
            return _roomTypeRepository.GetByID(id);
        }

        public void Update(RoomType entity)
        {
            _roomTypeRepository.Update(entity);
            _roomTypeRepository.Save();
        }
    }
}
