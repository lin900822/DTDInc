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
        }

        public override void OnLogic()
        {
            GameManager.Instance.GameUIManager.ShowMessage($"Game will start in {RoundManager.TimerRemainingTime:0}");
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

        private float _enterTime = 999999f;
        
        public override void EnterState()
        {
            RoundManager.StartGameOver();
            
            GameManager.Instance.GameUIManager.ShowMessage("Game Over !");

            _enterTime = Time.timeSinceLevelLoad;
        }

        public override void OnLogic()
        {
            if (Time.timeSinceLevelLoad - _enterTime >= 2f)
            {
                DetermineWinner();
                _enterTime = 999999f;
            }
        }

        public override void ExitState()
        {
            RoundManager.EndGameOver();
        }

        private void DetermineWinner()
        {
            var gameManager = GameManager.Instance;

            var winnerPlayerRef = gameManager.Coin.OwnerPlayerRef;

            if (winnerPlayerRef != default)
            {
                var winnerData = GameApp.Instance.GetPlayerNetworkData(winnerPlayerRef);

                var winnerName = winnerData.PlayerName;
            
                GameManager.Instance.GameUIManager.ShowMessage($"{winnerName} has won the Game !");
            }
            else
            {
                GameManager.Instance.GameUIManager.ShowMessage($"There is no winner !");
            }
        }
    }
}