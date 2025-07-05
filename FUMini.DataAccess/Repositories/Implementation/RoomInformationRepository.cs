using FUMini.BussinessObjects.Models;
using FUMini.DataAccess.Context;
using FUMini.DataAccess.Repositories.Base;
using FUMini.DataAccess.Repositories.Interfaces;

namespace FUMini.DataAccess.Repositories.Implementation
{
    public class RoomInformationRepository : GenericRepository<RoomInformation>, IRoomInformationRepository
    {
        private readonly FUMiniContext _context;
        public RoomInformationRepository(FUMiniContext context) : base(context)
        {
            _context = context;
        }
    }
}
