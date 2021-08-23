using BoardSystem;
using System;
using System.Collections.Generic;

namespace MoveSystem
{
   public  class Deck<TPiece> where TPiece: class, IPiece
    {
        private Dictionary<string, ICardAction<TPiece>> _actions = new Dictionary<string, ICardAction<TPiece>>();
        private List<string> _cards = new List<string>();
        private Random _random;

        public Deck()
        {
            _random = new Random();
        }

        public void Register(string name, ICardAction<TPiece> action)
        {
            if (_actions.ContainsKey(name))
            {
                return;
            }
            _actions.Add(name, action);
        }

        public string DrawCard()
        {
            if (_cards.Count <= 0)
                return null;

            var idx = _random.Next(0, _cards.Count);

            var name = _cards[idx];

            _cards.RemoveAt(idx);

            return name;
        }

        public void AddCart(string name, int amount = 1)
        {
            for (int cnt = 0; cnt < amount; cnt++)
                _cards.Add(name);
        }

        public ICardAction<TPiece> Action(string name)
        {
            if (_actions.TryGetValue(name, out var action))
            {
                return action;
            }
            return null;
        }
    }
}
