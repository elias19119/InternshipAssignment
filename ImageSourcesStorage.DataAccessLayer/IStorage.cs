namespace ImageSourcesStorage.DataAccessLayer
{
    using Microsoft.AspNetCore.Http;

    public interface IStorage
    {
        void Upload(IFormFile formfile);
    }
}
