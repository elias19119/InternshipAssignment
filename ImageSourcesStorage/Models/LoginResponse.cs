﻿namespace ImageSourcesStorage.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LoginResponse
    {
        public string Access_Token { get; set; }

        public string UserName { get; set; }
    }
}
