using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Views
{
    [CreateAssetMenu(fileName = "DefaultCardViewFactory", menuName = "GameSystem/CardViewFactory")]
    class CardViewFactory: ScriptableObject
    {
        [SerializeField]
        private List<CardView> _cardView = new List<CardView>();

        [SerializeField]
        private List<string> _cardNames = new List<string>();

        public CardView CreateCardView(string cardName)
        {
            var list = _cardView;
            var idex = _cardNames.IndexOf(cardName);

            var prefab = list[idex];
            prefab.gameObject.SetActive(true);
            var cardView = GameObject.Instantiate<CardView>(prefab);
            cardView.name = $"Spawned car ({cardName})";

            cardView.Card = cardName;
            return cardView;
        }
    }
}
