using BoardSystem;
using GameSystem.Models;
using UnityEngine;

namespace GameSystem.Views
{
    [CreateAssetMenu(fileName = "DefaultTileviewFactory", menuName = "GameSystem/TileView Factory")]
    public class TileViewFactory: ScriptableObject
    {
        [SerializeField]
        private TileView _hexTileView = null;

        [SerializeField]
        private PositionHelper _positionHelper = null;

        public TileView CreateTileView(Board<BoardPiece> board, Tile tile, Transform parent)
        {
            var position = _positionHelper.ToWorldPosition(tile.Position);

            var prefab = _hexTileView;

            var tileview = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);

            //tileview.Size = _positionHelper.TileSize;

            tileview.name = $"Tile {(char)(65 + tile.Position.X)}{tile.Position.X}";

            //tileview.Model = tile;

            return tileview;
        }
    }
}
