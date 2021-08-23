using BoardSystem;
using GameSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameSystem.CardCommands
{
    class MovementHelper
    {
        public delegate bool Validator(Board<BoardPiece> board, Tile fromtTile, Tile toTile);

        private Board<BoardPiece> _board;
        private Tile _playerTile;
        private List<Tile> _validTiles = new List<Tile>();

        public MovementHelper(Board<BoardPiece> board, Tile playerTile)
        {
            _board = board;
            _playerTile = playerTile;
        }

        public MovementHelper North(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(0, 1, step, validators);
        }

        public MovementHelper NorthEast(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(1, 1, step, validators);
        }

        public MovementHelper NorthWest(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(-1, 1, step, validators);
        }

        public MovementHelper East(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(1, 0, step, validators);
        }

        public MovementHelper SouthEast(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(1, -1, step, validators);
        }

        public MovementHelper South(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(0, -1, step, validators);
        }

        public MovementHelper SouthWest(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(-1, -1, step, validators);
        }

        public MovementHelper West(int step = int.MaxValue, params Validator[] validators)
        {
            return Collect(-1, 0, step, validators);
        }

        public MovementHelper Collect(int x, int z, int steps = int.MaxValue, params Validator[] validators)
        {
            Position MoveNext(Position position)
            {
                position.X += x;
                position.Z += z;

                return position;
            }

            var startTile = _playerTile;
            var startPosition = startTile.Position;

            var nextPosition = MoveNext(startPosition);

            int currentStep = 0;

            var blocked = false;
            while (!blocked && currentStep < steps)
            {
                var nextTile = _board.TileAt(nextPosition);
                if (nextTile == null)
                {
                    blocked = true;
                    break;
                }

                if (validators.All(v => v(_board, _playerTile, nextTile)))
                    _validTiles.Add(nextTile);

                nextPosition = MoveNext(nextPosition);
                currentStep++;
            }

            return this;
        }

        public MovementHelper Neighbors(Tile hex, int direction)
        {
            if (0 <= direction && direction < 6)
                _validTiles.Add(hex);

            return this;
        }

        public List<Tile> GenerateTiles()
        {
            return _validTiles;
        }

        public static bool CanCapture(Board<BoardPiece> board, BoardPiece character, Tile tile)
        {
            return board.PieceAt(tile) != null;
        }

        public static bool IsEmpty(Board<BoardPiece> board, BoardPiece character, Tile tile)
        {
            return board.PieceAt(tile) == null;
        }
    }
}

