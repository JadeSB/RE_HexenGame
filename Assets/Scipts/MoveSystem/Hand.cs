using BoardSystem;
using System;
using System.Collections.Generic;

namespace MoveSystem
{
    public class CardEventArgs : EventArgs
    {
        public string Name { get; }

        public CardEventArgs(string name)
        {
            Name = name;
        }
    }
    public class Hand<TPiece> where TPiece: class, IPiece
    {
        public event EventHandler<CardEventArgs> CardAdded;
        public event EventHandler<CardEventArgs> CardRemoved;

        private readonly Deck<TPiece> _deck;
        private readonly int _amount;

        private List<string> _card = new List<string>();

        public Hand(Deck<TPiece> deck, int amount)
        {
            _deck = deck;
            _amount = amount;
        }

        public void Remove(string name)
        {
            _card.Remove(name);
            OnCardRemoved(new CardEventArgs(name));
        }

        public void FillHand()
        {
            for (int cnt = _card.Count; cnt < _amount; cnt++)
                AddCard();
        }

        private void AddCard()
        {
            var name = _deck.DrawCard();
            if (name == null)
                return;

            _card.Add(name);

            OnCarAdded(new CardEventArgs(name));
        }

        protected virtual void OnCarAdded(CardEventArgs eventArgs)
        {
            var handler = CardAdded;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnCardRemoved(CardEventArgs eventArgs)
        {
            var handler = CardRemoved;
            handler?.Invoke(this, eventArgs);
        }
    }
}
