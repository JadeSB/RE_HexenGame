using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoardSystem
{
    public class Board<TPiece> where TPiece: class, IPiece
    {
        //verbinden zo de posities met de tiles op een efficiente manier
        private Dictionary<Position, Tile> _tiles = new Dictionary<Position, Tile>();

        public IList<Tile> Tiles => _tiles.Values.ToList();

        //maken twee lijsten aan omdat een Dictionary een one way street is en we moeten het kunnen gebruiken met de TPiece of met de Tile
        private List<TPiece> _values = new List<TPiece>();
        private List<Tile> _keys = new List<Tile>();

        public readonly int GridSize;

        public Board(int gridSize)
        {
            GridSize = gridSize;

            InitTiles();
        }

        private void InitTiles()
        {

            for (int x = -GridSize; -GridSize <= x && x <= GridSize; x++)
            {
                for (int y = -GridSize; -GridSize <= y && y <= GridSize; y++)
                {
                    var max = Mathf.Max(-GridSize, -x - GridSize);
                    var min = Mathf.Min(GridSize, -x + GridSize);

                    if (max <= y && y <= min)
                    {
                        _tiles.Add(new Position { X = x, Y = 0, Z = y }, new Tile(x, 0, y));

                    }

                }
            }
        }

        public Tile TileAt(Position position)
        {           
            if (_tiles.TryGetValue(position, out var tile))
                return tile;

            return null;
        }

        public TPiece PieceAt(Tile tile)
        {
            var idx = _keys.IndexOf(tile);
            if (idx == -1)
                return null;

            return _values[idx];
        }

        public Tile TileOf(TPiece piece)
        {
            var idx = _values.IndexOf(piece);

            if (idx == -1)
                return null;

            return _keys[idx];
        }

        public TPiece Take(Tile fromTile)
        {
            var idx = _keys.IndexOf(fromTile);
            if (idx == -1)
                return null;

            var piece = _values[idx];

            _values.RemoveAt(idx);
            _keys.RemoveAt(idx);

            piece.Taken();

            return piece;
        }

        public void Place(Tile toTile, TPiece character)
        {
            if (_keys.Contains(toTile))
                return;

            if (_values.Contains(character))
                return;

            _keys.Add(toTile);
            _values.Add(character);
        }

        public void Move(Tile fromtTile, Tile toTile)
        {
            var idx = _keys.IndexOf(fromtTile);

            if (idx == -1)
                return;

            var toPiece = PieceAt(toTile);
            if (toPiece != null)
                return;

            var piece = _values[idx];

            _values.RemoveAt(idx);
            _keys.RemoveAt(idx);
            Place(toTile, piece);

            piece.Moved(fromtTile, toTile);
        }

        public void Highlight(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.IsHighlighted = true;
            }
        }

        public void Unhighlight(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.IsHighlighted = false;
            }
        }
    }
}
