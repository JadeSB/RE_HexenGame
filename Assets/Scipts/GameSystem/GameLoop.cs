using BoardSystem;
using GameSystem.CardCommands;
using GameSystem.Models;
using GameSystem.States;
using GameSystem.Views;
using MoveSystem;
using StateSystem;
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
       
        private List<EnemyView> _enemyViews = new List<EnemyView>();

        public EnemyView CurrenPlayer;

        StateMachine<GameStateBase> _stateMachine;

        private void Update()
        {
            Debug.Log(CurrenPlayer);
        }

        private void Awake()
        {
            _stateMachine = new StateMachine<GameStateBase>();
            _deck.Register(PushBackCommand.NAME, new PushBackCommand());
            _deck.AddCart(PushBackCommand.NAME, 10);

            _deck.Register(LineAttackCommand.NAME, new LineAttackCommand());
            _deck.AddCart(LineAttackCommand.NAME, 10);

            _deck.Register(TeleportCommand.NAME, new TeleportCommand());
            _deck.AddCart(TeleportCommand.NAME, 10);

            _deck.Register(SwipeAttackCommand.NAME, new SwipeAttackCommand());
            _deck.AddCart(SwipeAttackCommand.NAME, 10);

            Hand = new Hand<BoardPiece>(_deck, 5);

           
            ConnectEnemyViewsToModel();
            ConnectHandViewsToModel();           
            Hand.FillHand();

            _stateMachine.RegisterState(GameStates.CardActivation, new CardGameState(Board, _deck, Hand));
            _stateMachine.RegisterState(GameStates.PionActivation, new PionSwitchGameState(_enemyViews, CurrenPlayer));
            _stateMachine.MoveTo(GameStates.CardActivation);
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

        public void Select(Tile hoverTile)
        {
            _stateMachine.CurrentState.Select(hoverTile);
        }        

        public void Select(string name)
        {
            _stateMachine.CurrentState.Select(name);
        }

        public  void OnDrop(Tile hoverTile)
        {

            _stateMachine.CurrentState.OnDrop(hoverTile);
        }

        public  void OnPointerExit(Tile model)
        {
            _stateMachine.CurrentState.OnPointerExit(model);
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

            CurrenPlayer = enemyViews[0];
            CurrenPlayer.IsPlayer = true;
            CurrenPlayer.ModelStatuesChanged();
        }
    }
}
