using Fusion;
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

        private bool hasCountDown = false;
        
        public override void EnterState()
        {
            RoundManager.StartReady();
            GameManager.Instance.GameUIManager.ShowMessage_Rpc("遊戲即將開始...");
        }

        public override void OnLogic()
        {
            GameManager.Instance.GameUIManager.UpdateTimer(RoundManager.TimerRemainingTime + 1);
            
            if(RoundManager.TimerRemainingTime <= 5f && !hasCountDown)
            {
                GameManager.Instance.GameUIManager.StartCountDown_Rpc();

                hasCountDown = true;
            }
        }

        public override void ExitState()
        {
            hasCountDown = false;
            RoundManager.EndReady();
        }
    }

    public class InGameState : RoundState
    {
        public InGameState(RoundManager manager) : base(manager) { }
        
        public override void EnterState()
        {
            RoundManager.StartInGame();
        }

        public override void OnLogic()
        {
            GameManager.Instance.GameUIManager.UpdateTimer(RoundManager.TimerRemainingTime + 1);
        }

        public override void ExitState()
        {
            GameManager.Instance.GameUIManager.UpdateTimer(0f);
            
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
            
            GameManager.Instance.GameUIManager.ShowMessage_Rpc("Game Over !");

            _enterTime = Time.timeSinceLevelLoad;
        }

        public override void OnLogic()
        {
            if (Time.timeSinceLevelLoad - _enterTime >= 2.5f)
            {
                DetermineWinner();
                _enterTime = 999999f;
            }
        }

        public override void ExitState()
        {
            RoundManager.EndGameOver();
            GameApp.Instance.Runner.SetActiveScene("Result");
        }

        private void DetermineWinner()
        {
            PlayerNetworkData winnerPlayerData = null;

            float longestKeepTime = -1f;
            foreach (var data in GameApp.Instance.PlayerNetworkDataList)
            {
                if (data.Value.KeepCoinTime > longestKeepTime)
                {
                    winnerPlayerData = data.Value;
                    longestKeepTime = data.Value.KeepCoinTime;
                }
            }

            GameApp.Instance.WinnerData = winnerPlayerData;
        }
    }
}