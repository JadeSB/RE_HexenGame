using BoardSystem;
using GameSystem.Models;
using System.Collections.Generic;

namespace GameSystem.CardCommands
{
    class SwipeAttackCommand : AbstractCardAction
    {
        public const string NAME = "SwipeAttack";     
        private List<Tile> _hoverValidTiles = new List<Tile>();
       
        public override bool Execute(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            Tiles(board,playerTile, hoverTile);

            var enemyCounter = 0;

            foreach (Tile enemy in _hoverValidTiles)
            {
                var piece = board.PieceAt(enemy);
                if (piece != null)
                {
                    board.Take(enemy);
                    enemyCounter++;                    
                }

            }

            if (enemyCounter != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

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
