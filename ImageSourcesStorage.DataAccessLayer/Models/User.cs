using System;
using System.Collections.Generic;
using System.Text;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int Score
        {
            get
            {
                return Score;
            }
            set
            {
                Score = 10;
            }
        }
    }
}
