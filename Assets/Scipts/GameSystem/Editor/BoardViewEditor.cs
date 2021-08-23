using GameSystem;
using GameSystem.Views;


namespace BoardSystem.Editor
{
    [UnityEditor.CustomEditor(typeof(BoardView))]
    public class BoardViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (UnityEngine.GUILayout.Button("Create Board"))
            {
                var boardView = target as BoardView;

                var tileviewFactorySp = serializedObject.FindProperty("_tileViewFactory");
                var tileViewFactory = tileviewFactorySp.objectReferenceValue as TileViewFactory;

                var game = GameLoop.Instance;
                var board = game.Board;

                foreach (var tile in board.Tiles)
                {
                    tileViewFactory.CreateTileView(board, tile, boardView.transform);
                }
            }
        }
    }
}
