namespace ImageSourcesStorage.DataAccessLayer
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using ImageSourcesStorage.DataAccessLayer.Models;

    public class CredentailsRepository : ICredentials
    {
        private readonly DataContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentailsRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CredentailsRepository(DataContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentailsRepository"/> class.
        /// </summary>
        public CredentailsRepository()
        {
        }

        public async Task<bool> IsCredentailsValid(string username, string password)
        {
            return await this.context.Credentials.AnyAsync(a => a.Username == username && a.Password == password);
        }
    }
}
