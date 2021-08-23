using GameSystem.Models;
using System;
using UnityEngine;


namespace GameSystem.Views
{
    public class PlayerView: MonoBehaviour
    {
        [SerializeField]
        private PositionHelper _positionHelper = null;

        [SerializeField]
        private bool _isPlayer;

        public bool IsPlayer => _isPlayer;
        private BoardPiece _model;

        public BoardPiece Model
        {
            get => _model;
            set
            {
                _model = value;
                if (_model != null)
                {             
                    _model.PieceMoved += ModelMoved;
                    _model.PieceTaken += ModelTaken;
                }
            }
        }

        private void ModelMoved(object sender, BoardPiece.PieceMovedEventArgs e)
        {
            var worldPosition = _positionHelper.ToWorldPosition(e.To.Position);
            transform.position = worldPosition;

            Vector3 convertingPostion = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
            transform.position = convertingPostion;
        }

        private void ModelTaken(object sender, EventArgs e)
        {
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            Model = null;
        }
    }
}
