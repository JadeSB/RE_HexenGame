using BoardSystem;
using GameSystem.Models;
using System.Collections.Generic;

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
            _hoverValidTiles.Add(hoverTile);
            return _hoverValidTiles;
        }
    }
}
