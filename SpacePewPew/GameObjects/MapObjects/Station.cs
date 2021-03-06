﻿using System;
using SpacePewPew.DataTypes;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public class Station : IObject
    {
        public PlayerColor OwnerColor { get; set; }
        public int Income { get; set; }

        public Station()
        {
            OwnerColor = PlayerColor.None;
            IsPassable = true;
            IsDestructable = false;
            Income = 5;
        }

        public Station(PlayerColor color) : this()
        {
            OwnerColor = color;
        }

        public void Capture(PlayerColor color)
        {
            OwnerColor = color;
        }
    }
}
