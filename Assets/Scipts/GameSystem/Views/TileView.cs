using BoardSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Views
{
    [SelectionBase]
    public class TileView: MonoBehaviour, IPointerEnterHandler,IDropHandler, IPointerExitHandler
    {
        private MeshRenderer _meshRenderer;

        [SerializeField]
        private PositionHelper _positionHelper = null;

        [SerializeField]
        private Material _highlightMaterial;

        private Material _originalMaterial;

        private Tile _model;

        public Tile Model
        {
            get => _model;
            set
            {
                if (_model != null)
                    _model.HighlightStatusChanged -= ModelHighLightStatusChanged;

                _model = value;

                if (_model != null)
                    _model.HighlightStatusChanged += ModelHighLightStatusChanged;
            }
        }
        internal Vector3 Size
        {
            set
            {
                transform.localScale = Vector3.one;

                var meshRenderer = GetComponentInChildren<MeshRenderer>();
                var meshSize = meshRenderer.bounds.size;

                var ratioX = value.x / meshSize.x;
                var ratioY = value.y / meshSize.y;
                var ratioZ = value.z / meshSize.z;

                transform.localScale = new Vector3(ratioX, ratioY, ratioZ);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameLoop.Instance.OnDrop(Model);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameLoop.Instance.Select(Model);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameLoop.Instance.OnPointerExit(Model);
        }

        private void ModelHighLightStatusChanged(object sender, EventArgs e)
        {
            if (Model.IsHighlighted)
                _meshRenderer.material = _highlightMaterial;
            else
                _meshRenderer.material = _originalMaterial;
        }

        private void Start()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _originalMaterial = _meshRenderer.sharedMaterial;

            GameLoop.Instance.Initialized += OnGameInitialized;
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {
            var board = GameLoop.Instance.Board;
            var boardPosition = _positionHelper.ToBoardPosition(transform.position);
            var tile = board.TileAt(boardPosition);

            Model = tile;
        }
    }
}
