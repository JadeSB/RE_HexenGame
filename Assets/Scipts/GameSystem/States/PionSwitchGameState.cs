using GameSystem.Views;
using System.Collections.Generic;

namespace GameSystem.States
{
    class PionSwitchGameState : GameStateBase
    {
        private int _roundCounter = 0;

        private List<EnemyView> _enemyViews;

        public PionSwitchGameState(List<EnemyView> enemyViews, EnemyView currentPlayer)
        {
            _enemyViews = enemyViews;

        }

        public override void OnEnter()
        {
            RoundCounterUpdate();
            ChangePion();
        }

        private void RoundCounterUpdate()
        {
            if (_roundCounter == _enemyViews.Count - 1)
                _roundCounter -= _roundCounter;
            else
                _roundCounter++;
        }

        public void ChangePion()
        {
            var currentPlayer = GameLoop.Instance.CurrenPlayer;
            currentPlayer.IsPlayer = false;
            currentPlayer.ModelStatuesChanged();
            currentPlayer = _enemyViews[_roundCounter];

            var board = GameLoop.Instance.Board;
            var playerTile = board.TileOf(currentPlayer.Model);

            if (GameLoop.Instance.Board.PieceAt(playerTile) != null)
            {
                currentPlayer.IsPlayer = true;
                currentPlayer.ModelStatuesChanged();
                GameLoop.Instance.CurrenPlayer = currentPlayer;
                StateMachine.MoveTo(GameStates.CardActivation);
            }
            else if(GameLoop.Instance.Board.PieceAt(playerTile) == null)
            {
                RoundCounterUpdate();
                ChangePion();
            }            
        }
    }
}
