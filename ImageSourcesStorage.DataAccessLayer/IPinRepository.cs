namespace ImageSourcesStorage.DataAccessLayer
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPinRepository
    {
        Task<List<Pin>> GetAllPinsAsync();
    }
}
