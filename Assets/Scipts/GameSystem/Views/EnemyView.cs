using GameSystem.Models;
using System;
using UnityEngine;

namespace GameSystem.Views
{
    class EnemyView: MonoBehaviour
    {
        [SerializeField]
        private bool _isPlayer;

        [SerializeField]
        private PositionHelper _positionHelper = null;

        private BoardPiece _model;

        public bool IsPlayer => _isPlayer;

        public BoardPiece Model
        {
            get => _model;
            internal set
            {
                if (_model != null)
                {                
                    _model.PieceMoved -= ModelMoved;
                    _model.PieceTaken -= PieceTaken;
                }

                _model = value;

                if (_model != null)
                {
                    _model.PieceMoved += ModelMoved;
                    _model.PieceTaken += PieceTaken;
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

        private void PieceTaken(object sender, EventArgs e)
        {
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            Model = null;
        }
    }
}
