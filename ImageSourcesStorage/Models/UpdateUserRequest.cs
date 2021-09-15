namespace ImageSourcesStorage.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UpdateUserRequest
    {
        public string Name { get; set; }

        public int Score { get; set; }
    }
}
