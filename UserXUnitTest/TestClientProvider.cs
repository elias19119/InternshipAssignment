using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ImageSourcesStorage;
using ImageSourcesStorage.DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserXUnitTest
{
    public class TestClientProvider
    {
        public HttpClient Client { get; set; }
        public TestClientProvider()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = server.CreateClient();
        }
    }
}
