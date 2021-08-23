using BoardSystem;
using GameSystem.Models;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.CardCommands
{   
    class TeleportCommand : AbstractCardAction
    {
        public const string NAME = "Teleport";
        private List<Tile> _hoverValidTiles = new List<Tile>();
        public override bool Execute(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            Tiles(board, playerTile, hoverTile);

            board.Move(playerTile, hoverTile);

            return true;
        }

        public override List<Tile> Tiles(Board<BoardPiece> board, Tile playerTile, Tile hoverTile)
        {
            //_hoverValidTiles.Add(hoverTile);
            //return _hoverValidTiles;

            var fromTile = playerTile;

            List<Tile> NeighbourStrategy(Tile tile) => Neighbours(tile, board);

            var pf = new AStarPathFinding<Tile>(NeighbourStrategy, Distance, Distance);

            return pf.Path(playerTile, hoverTile);
        }

        private List<Tile> Neighbours(Tile tile, Board<BoardPiece> board)
        {
            var neighbours = new List<Tile>();
            var position = tile.Position;

            var upPosition = position;
            upPosition.Z += 1;
            var upTile = board.TileAt(upPosition);
            if (upTile != null && board.PieceAt(upTile) == null)
                neighbours.Add(upTile);

            var downPosition = position;
            downPosition.Z -= 1;
            var downTile = board.TileAt(downPosition);
            if (downTile != null && board.PieceAt(downTile) == null)
                neighbours.Add(downTile);

            var rightPosition = position;
            rightPosition.X += 1;
            var rightTile = board.TileAt(rightPosition);
            if (rightTile != null && board.PieceAt(rightTile) == null)
                neighbours.Add(rightTile);

            var leftPosition = position;
            leftPosition.X -= 1;
            var leftTile = board.TileAt(leftPosition);
            if (leftTile != null && board.PieceAt(leftTile) == null)
                neighbours.Add(leftTile);

            return neighbours;
        }

        private float Distance(Tile fromTile, Tile toTile)
        {
            var fromPosition = fromTile.Position;
            var toPosition = toTile.Position;

            var fromDistanceX = fromPosition.X - Mathf.Floor(fromPosition.Z / 4);
            var fromDistanceY = fromPosition.Z;

            var toDistanceX = toPosition.X - Mathf.Floor(toPosition.Z / 4);
            var toDistanceY = toPosition.Z;

            var DistanceX = toDistanceX - fromDistanceX;
            var DistnaceY = toDistanceY - fromDistanceY;

            var totalDistance = Mathf.Max(Mathf.Abs(DistanceX), Mathf.Abs(DistnaceY), Mathf.Abs(DistanceX + DistnaceY));

            return totalDistance;
        }
    }
}
