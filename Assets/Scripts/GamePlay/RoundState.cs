using UnityEngine;

namespace GamePlay
{
    public abstract class RoundState
    {
        protected readonly RoundManager RoundManager = null;

        protected RoundState(RoundManager manager)
        {
            RoundManager = manager;
        }

        public virtual void EnterState() { }

        public virtual void OnLogic() { }

        public virtual void ExitState() { }
    }

    public class ReadyState : RoundState
    {
        public ReadyState(RoundManager manager) : base(manager) { }

        public override void EnterState()
        {
            RoundManager.StartReady();
            
            GameManager.Instance.GameUIManager.ShowMessage("Ready...");
        }

        public override void OnLogic()
        {
            
        }

        public override void ExitState()
        {
            RoundManager.EndReady();
        }
    }

    public class InGameState : RoundState
    {
        public InGameState(RoundManager manager) : base(manager) { }
        
        public override void EnterState()
        {
            RoundManager.StartInGame();
            
            GameManager.Instance.GameUIManager.ShowMessage("Game Start !");
        }

        public override void OnLogic()
        {
            
        }

        public override void ExitState()
        {
            RoundManager.EndInGame();
        }
    }
    
    public class GameOverState : RoundState
    {
        public GameOverState(RoundManager manager) : base(manager) { }
        
        public override void EnterState()
        {
            RoundManager.StartGameOver();
            
            GameManager.Instance.GameUIManager.ShowMessage("Game Over !");
        }

        public override void OnLogic()
        {
            
        }

        public override void ExitState()
        {
            RoundManager.EndGameOver();
        }
    }
}