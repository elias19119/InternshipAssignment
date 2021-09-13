using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageSourcesStorage.DataAccessLayer
{
    public interface IGenericRepository<TUser> where TUser: class
    {
        IEnumerable<TUser> GetAll();

        Task<TUser> GetById(Guid id);

        Task Create(TUser entity);

        Task Update(Guid Id, TUser entity);

        Task Delete(Guid Id);
    }
}
