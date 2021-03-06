﻿using Battleships.Service.Extensions;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Models
{
    public abstract class Ship
    {
        public abstract ShipType ShipType { get; }

        public abstract int Size { get; }

        public OrientationType Orientation { get; set; }

        public GameField StartField { get; set; }

        public IEnumerable<GameField> Fields { get; private set; }

        protected Ship(OrientationType orientation, GameField startField)
        {
            Orientation = orientation;
            StartField = startField ?? throw new ArgumentNullException(nameof(startField));
            Fields = StartField.Expand(Orientation, Size);
        }

        internal bool IsSunk() => Fields.All(f => f.WasHit);
    }
}