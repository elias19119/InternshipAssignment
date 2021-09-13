using System;
using System.Collections.Generic;
using System.Text;

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public User() { }
        public User(Guid UserId, string name, int score)
        {
            this.UserId = UserId;
            this.Name = name;
            this.Score = score;
        }
    }
}
