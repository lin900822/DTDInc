using System;
using Fusion;
using UnityEngine;

namespace GamePlay
{
    public class MatchManager : NetworkBehaviour
    {
        public event Action OnReadyStarted  = null;
        public event Action OnReadyEnd      = null;
        public event Action OnInGameStarted = null; 
        public event Action OnInGameEnd     = null;
        public event Action OnGameOverStart = null;
        public event Action OnGameOverEnd   = null;

        [SerializeField] private float readyTime    = 5f;
        [SerializeField] private float inGameTime   = 120f;
        [SerializeField] private float gameOverTime = 5f;

        [Networked] public RoundStage Stage { get; set; } = RoundStage.None;
        [Networked] public TickTimer RoundTimer { get; set; }

        public override void FixedUpdateNetwork()
        {
            if (RoundTimer.Expired(Runner))
            {
                SwitchStage();
            }
        }

        private void SwitchStage()
        {
            switch (Stage)
            {
                case RoundStage.Ready:
                    Stage = RoundStage.Ready;
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, readyTime);
                
                    EndGameOver();
                    StartReady();
                    break;
            
                case RoundStage.InGame:
                    Stage = RoundStage.InGame;
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, inGameTime);
                
                    EndReady();
                    StartInGame();
                    break;
            
                case RoundStage.GameOver:
                    Stage = RoundStage.GameOver;
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, gameOverTime);
                
                    EndInGame();
                    StartGameOver();
                    break;
            }
        }

        public void Init()
        {
            Stage = RoundStage.Ready;
            RoundTimer = TickTimer.CreateFromSeconds(Runner, readyTime);
                
            StartReady();
        }
    
        private void StartReady()
        {
            OnReadyStarted?.Invoke();
        }

        private void EndReady()
        {
            OnReadyEnd?.Invoke();
        }
    
        private void StartInGame()
        {
            OnInGameStarted?.Invoke();
        }
    
        private void EndInGame()
        {
            OnInGameEnd?.Invoke();
        }

        private void StartGameOver()
        {
            OnGameOverStart?.Invoke();
        }

        private void EndGameOver()
        {
            OnGameOverEnd?.Invoke();
        }
    }
}