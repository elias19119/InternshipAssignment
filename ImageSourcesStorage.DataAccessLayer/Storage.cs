namespace ImageSourcesStorage.DataAccessLayer
{
    using Azure.Storage.Blobs;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    public class Storage : IStorage
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly IConfiguration configuration;

        public Storage(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            this.blobServiceClient = blobServiceClient;
            this.configuration = configuration;
        }

        public void Upload(IFormFile formfile)
        {
            var containerName = configuration.GetSection("Storage:ContainerName").Value;

            var containerclient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobclient = containerclient.GetBlobClient(formfile.FileName);

            using var stream = formfile.OpenReadStream();
            blobclient.Upload(stream, true);
        }
    }
}
