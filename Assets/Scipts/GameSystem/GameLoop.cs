using BoardSystem;
using GameSystem.CardCommands;
using GameSystem.Models;
using GameSystem.Views;
using MoveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameSystem
{
    public class GameLoop : SingletonMonoBehaviour<GameLoop>
    {
        public event EventHandler Initialized;

        [SerializeField]
        private PositionHelper _positionHelper = null;

        public Board<BoardPiece> Board { get; } = new Board<BoardPiece>(3);
        private string _cardName;
        private ICardAction<BoardPiece> _cardAction;
        private Deck<BoardPiece> _deck = new Deck<BoardPiece>();
        public Hand<BoardPiece> Hand = null;
        public HandView HandView = null;

        private List<Tile> _validTiles = new List<Tile>();

        //private PlayerView _playerView;
        private List<EnemyView> _enemyViews = new List<EnemyView>();

        public EnemyView CurrentPlayer;

        private void Awake()
        {

            _deck.Register(PushBackCommand.NAME, new PushBackCommand());
            _deck.AddCart(PushBackCommand.NAME, 10);

            _deck.Register(LineAttackCommand.NAME, new LineAttackCommand());
            _deck.AddCart(LineAttackCommand.NAME, 10);

            _deck.Register(TeleportCommand.NAME, new TeleportCommand());
            _deck.AddCart(TeleportCommand.NAME, 10);

            _deck.Register(SwipeAttackCommand.NAME, new SwipeAttackCommand());
            _deck.AddCart(SwipeAttackCommand.NAME, 10);

            Hand = new Hand<BoardPiece>(_deck, 5);

            //ConnectCharacterViewsToModel();
            ConnectEnemyViewsToModel();
            ConnectHandViewsToModel();           
            Hand.FillHand();
        }

        private void Start()
        {
            StartCoroutine(PostStart());           
        }

        private void ConnectHandViewsToModel()
        {
            HandView = FindObjectOfType<HandView>();
            HandView.Model = Hand;
        }

        private IEnumerator PostStart()
        {
            yield return new WaitForEndOfFrame();

            OnInitialized(EventArgs.Empty);
        }

        protected virtual void OnInitialized(EventArgs arg)
        {
            EventHandler handler = Initialized;
            handler?.Invoke(this, arg);
        }

        //private void ConnectCharacterViewsToModel()
        //{
        //    _playerView = FindObjectOfType<PlayerView>();

        //    var worldPosition = _playerView.transform.position;
        //    var boardPosition = _positionHelper.ToBoardPosition(worldPosition);

        //    var tile = Board.TileAt(boardPosition);

        //    var character = new BoardPiece(_playerView.IsPlayer);
        //    _playerView.Model = character;

        //    Board.Place(tile, character);

        //}

        public void Select(Tile hoverTile)
        {

            if (_cardAction == null)
                return;

            _validTiles = _cardAction.Tiles(Board, GetPlayerTile(), hoverTile);
            Board.Highlight(_validTiles);
        }

        private Tile GetPlayerTile()
        {
            return Board.TileOf(CurrentPlayer.Model);
        }

        public void Select(string name)
        {
            _cardName = name;
            _cardAction = _deck.Action(name);
        }

        public  void OnDrop(Tile hoverTile)
        {

            if (_validTiles.Contains(hoverTile))
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
            else
            {
                _cardAction = null;
            }           
        }

        public  void OnPointerExit(Tile model)
        {
            Board.Unhighlight(_validTiles);
            _validTiles.Clear();
        }

        private void ConnectEnemyViewsToModel()
        {
            var enemyViews = FindObjectsOfType<EnemyView>();
            foreach (var enemyView in enemyViews)
            {
                var worldPosition = enemyView.transform.position;
                var boardPosition = _positionHelper.ToBoardPosition(worldPosition);

                var tile = Board.TileAt(boardPosition);
                var enemy = new BoardPiece(enemyView.IsPlayer);

                Board.Place(tile, enemy);
                enemyView.Model = enemy;

                _enemyViews.Add(enemyView);
            }

            CurrentPlayer = enemyViews[0];
        }
    }
}
