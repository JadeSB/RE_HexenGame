
using BoardSystem;
using System.Collections.Generic;

namespace MoveSystem
{
    public interface ICardAction<TPiece> where TPiece : class, IPiece
    {
        List<Tile> Tiles(Board<TPiece> board, Tile playerTile, Tile hoverTile);

        bool Execute(Board<TPiece> board, Tile playerTile, Tile hoverTile);
    }
}
