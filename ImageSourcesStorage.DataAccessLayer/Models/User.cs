namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public int Score { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="score"></param>
        public User(Guid userId, string name, int score)
        {
            this.UserId = userId;
            this.Name = name;
            this.Score = score;
        }
    }
}
