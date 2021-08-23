using BoardSystem;
using GameSystem.Models;
using GameSystem.Views;
using System;
using System.Collections.Generic;

namespace GameSystem.CardCommands
{
    class LineAttackCommand : AbstractCardAction
    {
        public const string NAME = "LineAttack";      
        private List<Tile> _hoverValidTiles = new List<Tile>();

        public override bool Execute(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            var hoverTiles = Tiles(board,playerTile, hoverTile);
            foreach (var enemy in hoverTiles)
            {
                var toPiece = board.PieceAt(enemy);

                if (toPiece != null)
                {
                    board.Take(enemy);
                }
            }

            return true;
        }

        public override List<Tile> Tiles(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            var validTiles = new MovementHelper(board, playerTile)
               .North()
               .East()
               .SouthEast()
               .South()
               .West()
               .NorthWest()
               .GenerateTiles();

            if (validTiles.Contains(hoverTile))
            {
                var position = new Position
                {
                    X = hoverTile.Position.X - playerTile.Position.X,
                    Y = hoverTile.Position.Y - playerTile.Position.Y,
                    Z = hoverTile.Position.Z - playerTile.Position.Z
                };

                var normPosition = Normalize(position);
                bool blocked = false;

                var nextPosition = playerTile.Position;
                while (!blocked)
                {
                    nextPosition = nextPosition + normPosition;
                    var nextTile = board.TileAt(nextPosition);
                    if (nextTile == null)
                    {
                        blocked = true;
                    }
                    else
                    {
                        _hoverValidTiles.Add(nextTile);
                    }
                }
                return _hoverValidTiles;
            }
            else
            {
                return validTiles;
            }
        }        
    }
}
