// <copyright file="Board.cs" company="INDG">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;

    public class Board
    {
        public Guid BoardId { get; set; }

        public User Owner { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public List<Pin> Pins { get; set; }
    }
}
