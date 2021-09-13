using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ImageSourcesStorage.Controllers;
using ImageSourcesStorage.DataAccessLayer;
using ImageSourcesStorage.DataAccessLayer.Models;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace UserXUnitTest
{
    public class UserTest
    {
        public Mock<IUserRepository> mock = new Mock<IUserRepository>();
        [Fact]
        public async Task Get_WhenCalled_ReturnsAllUsers()
        {
            var client = new TestClientProvider().Client;
            var response = await client.GetAsync("api/users");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Post_WhenCalled_PostUser()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("api/users",
                    new StringContent(JsonConvert.SerializeObject(new User(new Guid("85ea3517-f2a3-4dfb-8817-777326053ce0"), "elias", 25))));
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
