using BoardSystem;
using GameSystem.Models;
using GameSystem.Views;
using MoveSystem;
using System;
using System.Collections.Generic;

namespace GameSystem.CardCommands
{
    public abstract class AbstractCardAction : ICardAction<BoardPiece>
    {
        public abstract bool Execute(Board<BoardPiece> board, Tile playerTile, Tile hoverTile);

        public abstract List<Tile> Tiles(Board<BoardPiece> board, Tile playerTile, Tile hoverTile);

        public Position Normalize(Position position)  //dit normalizeerd de richting
        {
            var x = position.X != 0 ? position.X / Math.Abs(position.X) : 0;
            var z = position.Z != 0 ? position.Z / Math.Abs(position.Z) : 0;
            return new Position() { X = x, Z = z };
        }
    }

}

