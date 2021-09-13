using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ImageSourcesStorage.DataAccessLayer
{
    public class GenericReopsitory <TUser> : IGenericRepository<User> where TUser: class
    {
        private DataContext _dataContext;
        public GenericReopsitory(DataContext DataContext)
        {
            this._dataContext = DataContext;
        }
        public IEnumerable<User> GetAll()
        {
            return _dataContext.Set<User>().AsNoTracking();
        }

        public async Task<User> GetById(Guid Id)
        {
            return await _dataContext.Set<User>().FirstOrDefaultAsync(e=>e.UserId == Id);
        }

        public async Task Create(User user)
        {
            await _dataContext.Set<User>().AddAsync(user);
            await SaveAsync();
        }

        public async Task Update(Guid Id, User user)
        {
             _dataContext.Set<User>().Update(user);
            await SaveAsync();
        }

        public async Task Delete(Guid Id)
        {
            var user = await GetById(Id);
            _dataContext.Set<User>().Remove(user);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
