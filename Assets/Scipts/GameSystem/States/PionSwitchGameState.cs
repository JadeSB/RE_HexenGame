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
            if (_roundCounter == _enemyViews.Count)
                _roundCounter -= _roundCounter;
            else
                _roundCounter++;

            ChangePion();
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
            }
            else
            {
                _roundCounter++;
                ChangePion();
            }

            GameLoop.Instance.CurrenPlayer = currentPlayer;
            StateMachine.MoveTo(GameStates.CardActivation);
        }
    }
}
