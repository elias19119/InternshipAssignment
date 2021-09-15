// <copyright file="PinBoard.cs" company="INDG">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ImageSourcesStorage.DataAccessLayer.Models
{
    using System;

    public class PinBoard
    {
        public Guid PinBoardId { get; set; }

        public Guid PinId { get; set; }

        public Guid BoardId { get; set; }
    }
}
