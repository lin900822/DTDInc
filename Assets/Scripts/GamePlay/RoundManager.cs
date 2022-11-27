using System;
using Fusion;
using UnityEngine;

namespace GamePlay
{
    public class RoundManager : NetworkBehaviour
    {
        public event Action OnReadyStarted  = null;
        public event Action OnReadyEnd      = null;
        public event Action OnInGameStarted = null; 
        public event Action OnInGameEnd     = null;
        public event Action OnGameOverStart = null;
        public event Action OnGameOverEnd   = null;
        
        [HideInInspector] public float TimerRemainingTime => RoundTimer.RemainingTime(Runner).Value;
        
        [SerializeField] private float readyTime    = 5f;
        [SerializeField] private float inGameTime   = 120f;
        [SerializeField] private float gameOverTime = 5f;

        [Networked] public RoundStage Stage     { get; set; } = RoundStage.None;
        [Networked] public TickTimer RoundTimer { get; set; }

        private RoundState _state         = null;

        private RoundState _readyState    = null;
        private RoundState _inGameState   = null;
        private RoundState _gameOverState = null;

        private void Awake()
        {
            _readyState     = new ReadyState(this);
            _inGameState    = new InGameState(this);
            _gameOverState  = new GameOverState(this);
        }

        public override void Spawned()
        {
            SwitchStage();
        }

        public override void FixedUpdateNetwork()
        {
            if (RoundTimer.Expired(Runner))
            {
                SwitchStage();
            }

            DoStateLogic();
        }

        private void DoStateLogic()
        {
            if (_state != null)
            {
                _state.OnLogic();
            }
        }

        private void SwitchStage()
        {
            switch (Stage)
            {
                case RoundStage.Ready:
                    Stage = RoundStage.InGame;
                    SetState(_inGameState);
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, inGameTime);
                    break;
            
                case RoundStage.InGame:
                    Stage = RoundStage.GameOver;
                    SetState(_gameOverState);
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, gameOverTime);
                    break;
            
                case RoundStage.GameOver:
                    Stage = RoundStage.Ready;
                    SetState(_readyState);
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, readyTime);
                    break;
                
                default:
                    Stage = RoundStage.Ready;
                    SetState(_readyState);
                    RoundTimer = TickTimer.CreateFromSeconds(Runner, readyTime);
                    break;
            }
        }

        private void SetState(RoundState state)
        {
            if (_state == state) return;
            
            if(_state != null) { _state.ExitState(); }

            _state = state;
            
            if(_state != null) { _state.EnterState(); }
        }
    
        public void StartReady()    => OnReadyStarted?.Invoke();
        public void EndReady()      => OnReadyEnd?.Invoke();
        public void StartInGame()   => OnInGameStarted?.Invoke();
        public void EndInGame()     => OnInGameEnd?.Invoke();
        public void StartGameOver() => OnGameOverStart?.Invoke();
        public void EndGameOver()   => OnGameOverEnd?.Invoke();
    }
}