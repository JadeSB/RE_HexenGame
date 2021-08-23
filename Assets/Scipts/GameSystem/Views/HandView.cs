using GameSystem.Models;
using MoveSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Views
{
    public class HandView:MonoBehaviour
    {
        [SerializeField]
        private CardViewFactory _cardViewFactory;

        private Hand<BoardPiece> _hand;

        private List<string> _cards = new List<string>();
        private List<CardView> _cardViews = new List<CardView>();

        public Hand<BoardPiece> Model
        {
            get => _hand;
            set
            {
                _hand = value;
                _hand.CardAdded += OnHandCardAdded;
                _hand.CardRemoved += OnHanCardRemoved;
            }
        }

        private void OnHanCardRemoved(object sender, CardEventArgs e)
        {
            var idx = _cards.IndexOf(e.Name);

            var cardView = _cardViews[idx];

            _cards.RemoveAt(idx);
            _cardViews.RemoveAt(idx);

            cardView.RemoveCard();

        }

        private void OnHandCardAdded(object sender, CardEventArgs e)
        {
            var cardView = _cardViewFactory.CreateCardView(e.Name);
            _cards.Add(e.Name);
            _cardViews.Add(cardView);
            cardView.transform.parent = this.transform;

        }

        public void AddCard()
        {
            GameLoop.Instance.Hand.FillHand();
        }
    }
}
