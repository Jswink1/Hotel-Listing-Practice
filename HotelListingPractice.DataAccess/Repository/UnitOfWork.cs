using HotelListingPractice.DataAccess.Data;
using HotelListingPractice.DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingPractice.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IRepository<Country> _countries;
        private IRepository<Hotel> _hotels;
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        // if countries is null, return a new Repository<Country>
        public IRepository<Country> Countries => _countries ??= new Repository<Country>(_context);
        // if hotels is null, return a new Repository<Hotel>
        public IRepository<Hotel> Hotels => _hotels ??= new Repository<Hotel>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
