using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageSourcesStorage.Models
{
    public class AddUserResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserResponse"/> class.
        /// </summary>
        /// <param name="userId"></param>
        public AddUserResponse(Guid userId)
        {
            this.userId = userId;
        }

        public Guid userId { get; set; }
    }
}
