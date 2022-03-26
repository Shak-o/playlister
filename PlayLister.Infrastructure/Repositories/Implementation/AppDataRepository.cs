using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayLister.Infrastructure.Context;
using PlayLister.Infrastructure.Models;
using PlayLister.Infrastructure.Repositories.Interfaces;

namespace PlayLister.Infrastructure.Repositories.Implementation
{
    public class AppDataRepository : IAppDataRepository
    {
        private readonly PlayListerContext _context;

        public AppDataRepository(PlayListerContext context)
        {
            _context = context;
        }

        public AppData GetData()
        {
            return _context.AppData.SingleOrDefault(x => x.ClientId != "youtube");
        }

        public AppData GetKey()
        {
            return _context.AppData.SingleOrDefault(x => x.ClientId == "youtube");
        }
    }
}
