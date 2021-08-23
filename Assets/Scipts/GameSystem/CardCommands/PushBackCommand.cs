using BoardSystem;
using GameSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSystem.CardCommands
{
    class PushBackCommand : AbstractCardAction
    {
        public const string NAME = "PushBack";
        private List<Tile> _hoverValidTiles = new List<Tile>();
        public override bool Execute(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            //Verwijderen van de enemies op de execute van de kaart
            var hoverTiles = Tiles(board, playerTile, hoverTile);
            foreach (var enemyTile in hoverTiles)
            {
                var toPiece = board.PieceAt(enemyTile);

                if (toPiece != null)
                {
                    var normPosition = Normalize(enemyTile.Position);
                    var toTilePostion = new Position
                    {
                        X = enemyTile.Position.X + normPosition.X,
                        Y = enemyTile.Position.Y + normPosition.Y,
                        Z = enemyTile.Position.Z + normPosition.Z
                    };
                    board.Move(enemyTile, board.TileAt(toTilePostion));
                }
            }

            return true;
        }

        public override List<Tile> Tiles(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            var validTiles = new MovementHelper(board, playerTile)
               .North(1)
               .East(1)
               .SouthEast(1)
               .South(1)
               .West(1)
               .NorthWest(1)
               .GenerateTiles();

            if (!validTiles.Contains(hoverTile))
                return validTiles;

            if (validTiles.Contains(hoverTile))
            {
                var position = hoverTile.Position - playerTile.Position;

                FindingNeighbours(board, playerTile, position);
                var getNeighbours = FindingNeighbours(board, playerTile, position);
                _hoverValidTiles = new List<Tile>() { hoverTile };
                foreach (var neighbourPosition in getNeighbours)
                {
                    _hoverValidTiles.Add(neighbourPosition);
                }

            }
            return _hoverValidTiles;
        }

        private List<Tile> FindingNeighbours(Board<BoardPiece> board, Tile playerTile, Position position)
        {
            switch (position.X)
            {
                case 0:
                    switch (position.Z)
                    {
                        case -1:
                            _hoverValidTiles = new MovementHelper(board, playerTile).
                            West(1).
                            SouthEast(1).
                            GenerateTiles();
                            break;

                        case 1:
                            _hoverValidTiles = new MovementHelper(board, playerTile).
                            East(1).
                            NorthWest(1).
                            GenerateTiles();
                            break;
                    }
                    break;
                case 1:
                    switch (position.Z)
                    {
                        case -1:
                            _hoverValidTiles = new MovementHelper(board, playerTile).
                            South(1).
                            East(1).
                            GenerateTiles();
                            break;

                        case 0:
                            _hoverValidTiles = new MovementHelper(board, playerTile).
                            North(1).
                            SouthEast(1).
                            GenerateTiles();
                            break;
                    }
                    break;

                case -1:
                    switch (position.Z)
                    {
                        case +1:
                            _hoverValidTiles = new MovementHelper(board, playerTile).
                            North(1).
                            West(1).
                            GenerateTiles();
                            break;
                        case 0:
                            _hoverValidTiles = new MovementHelper(board, playerTile).
                            South(1).
                            NorthWest(1).
                            GenerateTiles();
                            break;
                    }
                    break;
            }
            return _hoverValidTiles;
        }
    }
}
