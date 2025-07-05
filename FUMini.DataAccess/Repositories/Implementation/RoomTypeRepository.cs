using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Base;
using FUMini.DataAccess.Repositories.Interfaces;

namespace FUMini.DataAccess.Repositories.Implementation
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
    {
        private readonly FUMiniContext _context;

        public RoomTypeRepository(FUMiniContext context) : base(context)
        {
            _context = context;
        }
    }
}
