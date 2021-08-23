using BoardSystem;
using UnityEngine;

namespace GameSystem.Views
{
    [CreateAssetMenu(fileName = "DefaultPositionHelper", menuName = "GameSystem/PositionHelper")]
    class PositionHelper: ScriptableObject
    {
        public Vector3 TileSize => _tileSize;

        [SerializeField]
        private Vector3 _tileSize = Vector3.one;

        private float _yOffset = 0.6667f;
        private float _xOffset = 0.3333f;


        public Position ToBoardPosition(Vector3 worldPosition)
        {
            //pixeltohex
            var x = (Mathf.Sqrt(3) / 3 * worldPosition.x + _xOffset * worldPosition.z) / TileSize.x;
            var y = _yOffset * worldPosition.z / TileSize.x;
            y = -y;

            var boardPosition = new Position
            {
                X = Mathf.RoundToInt(x),
                Y = (int)worldPosition.y,
                Z = Mathf.RoundToInt(y)
            };
            return boardPosition;
        }

        public Vector3 ToWorldPosition(Position boardPosition)
        {
            //hex to pixel  
            var x = TileSize * (Mathf.Sqrt(3) * boardPosition.X + Mathf.Sqrt(3) / 2 * boardPosition.Z);
            var y = TileSize * (3 / 2 * boardPosition.Z);

            var tilePosition = new Vector3(x.x, boardPosition.Y, y.z + (y.x / 2));

            return tilePosition;

        }
    }
}

