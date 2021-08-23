using BoardSystem;
using StateSystem;

namespace GameSystem.States
{
    public abstract class GameStateBase : IState<GameStateBase>
    {
        public StateMachine<GameStateBase> StateMachine { get; set; }
        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Select(Tile hoverTile) { }
        public virtual void Select(string name) { }

        public virtual void OnDrop(Tile hoverTile) { }

        public virtual void OnPointerExit(Tile model) { }
    }
}
