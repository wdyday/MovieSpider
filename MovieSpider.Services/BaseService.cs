using MovieSpider.Data;
using MovieSpider.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services
{
    public abstract class BaseService : IDisposable
    {
        protected SpiderDbContext _context = null;
        protected SpiderDbContext db
        {
            get { return _context; }
        }

        public BaseService()
        {
            _context = new SpiderDbContext();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
