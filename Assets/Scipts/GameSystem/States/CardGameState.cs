
using BoardSystem;
using GameSystem.Models;
using GameSystem.Views;
using MoveSystem;
using System.Collections.Generic;

namespace GameSystem.States
{
    class CardGameState: GameStateBase
    {
        private List<Tile> _validTiles = new List<Tile>();

        private ICardAction<BoardPiece> _cardAction;
        private string _cardName;

        public Board<BoardPiece> Board;
        private Deck<BoardPiece> _deck;
        public Hand<BoardPiece> Hand = null;
        private EnemyView _currentPlayer;

        public CardGameState(Board<BoardPiece> board, EnemyView currentPlayer, Deck<BoardPiece> deck, Hand<BoardPiece> hand)
        {
            Board = board;       
            _deck = deck;
            Hand = hand;
            _currentPlayer = currentPlayer;
        }

        public override void Select(Tile hoverTile)
        {
            if (_cardAction == null)
                return;

            _validTiles = _cardAction.Tiles(Board, GetPlayerTile(), hoverTile);
            Board.Highlight(_validTiles);
        }

        private Tile GetPlayerTile()
        {
            return Board.TileOf(_currentPlayer.Model);
        }
        public override void Select(string name)
        {
            _cardName = name;
            _cardAction = _deck.Action(name);
        }

        public override void OnDrop(Tile hoverTile)
        {
            Board.Unhighlight(_validTiles);
            _validTiles.Clear();

            if (_cardAction == null)
                return;

            if (_cardAction.Execute(Board, GetPlayerTile(), hoverTile))
            {
                Hand.Remove(_cardName);
                _cardAction = null;
                Hand.FillHand();
            }
        }

        public override void OnPointerExit(Tile model)
        {
            Board.Unhighlight(_validTiles);
            _validTiles.Clear();
        }
    }
}
