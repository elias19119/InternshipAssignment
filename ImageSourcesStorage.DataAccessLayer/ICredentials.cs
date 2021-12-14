namespace ImageSourcesStorage.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public interface ICredentials
    {
        Task<bool> IsCredentailsValid(string username, string password);
    }
}
