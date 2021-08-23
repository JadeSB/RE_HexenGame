using System;

namespace BoardSystem
{
   public class Tile
    {
        public event EventHandler HighlightStatusChanged;

        private bool _IsHighlighted = false;

        public Position Position { get; }

        public Tile(int x, int y, int z)
        {
            Position = new Position { X = x, Y = y, Z = z };
        }

        public bool IsHighlighted
        {
            get => _IsHighlighted;
            internal set
            {
                _IsHighlighted = value;
                OnHighLightStatusChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnHighLightStatusChanged(EventArgs args)
        {
            EventHandler handler = HighlightStatusChanged;
            handler?.Invoke(this, args);
        }
    }
}
