using BoardSystem;
using GameSystem.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Models
{
    public class BoardPiece: IPiece
    {
        public class PieceMovedEventArgs : EventArgs
        {
            public Tile From { get; }
            public Tile To { get; }

            public PieceMovedEventArgs(Tile from, Tile to)
            {
                From = from;
                To = to;
            }
        }

        public event EventHandler<PieceMovedEventArgs> PieceMoved;
        public event EventHandler PieceTaken;

        public event EventHandler PlayerStatusChanged;

        private bool _isPlayer = false;

        public bool HasMoved { get; set; } = false;

        public bool IsPlayer {
            get => _isPlayer;
            internal set 
            {
                OnPlayerStatuesChanged(EventArgs.Empty);
            }
           
        }

        protected virtual void OnPlayerStatuesChanged(EventArgs args)
        {
            EventHandler handler = PlayerStatusChanged;
            handler?.Invoke(this, args);
        }

        public BoardPiece(bool isPlayer)
        {
            IsPlayer = isPlayer;
        }

        void IPiece.Moved(Tile fromTile, Tile toTile)
        {
            OnPieceMoved(new PieceMovedEventArgs(fromTile, toTile));
        }

        private void OnPieceMoved(PieceMovedEventArgs arg)
        {
            EventHandler<PieceMovedEventArgs> handler = PieceMoved;
            handler?.Invoke(this, arg);
        }
       
        private void OnPieceTaken(EventArgs args)
        {
            EventHandler handler = PieceTaken;
            handler?.Invoke(this, args);
        }

        public void Taken()
        {
            OnPieceTaken(EventArgs.Empty);
        }        
    }
}
