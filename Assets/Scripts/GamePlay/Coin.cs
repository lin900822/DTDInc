using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class Coin : NetworkBehaviour
    {
        [SerializeField] private float sceneRadius = 45f;
        [SerializeField] private float detectRadius = 1f;

        [SerializeField] private LayerMask hitLayers = default;

        private PlayerController _playerController = null;

        private readonly List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();

        [Networked] public NetworkBehaviourId OwnerId { get; set; }

        private GameManager _gameManager = null;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameManager.RoundManager.Stage != RoundStage.InGame) return;
            
            DetectPlayer();
            
            _playerController = Runner.TryFindBehaviour(OwnerId, out PlayerController obj) ? obj : null;

            FollowPlayer();

            DetectIfIsOutOfBound();
        }

        private void DetectIfIsOutOfBound()
        {
            if (transform.position.y < 0)
            {
                ResetCoin();
            }
        }

        private void DetectPlayer()
        {
            if (!Object.HasStateAuthority) return;
            if (_playerController != null) return;

            _hits.Clear();

            Runner.LagCompensation.OverlapSphere(transform.position, detectRadius, Object.InputAuthority, _hits, hitLayers, HitOptions.None);

            if (_hits.Count > 0)
            {
                if (_hits[0].GameObject.TryGetComponent<PlayerController>(out var obj))
                {
                    OwnerId = obj.Id;
                }
            }
        }
        
        public override void Render()
        {
            if (_gameManager.RoundManager.Stage != RoundStage.InGame) return;
            
            FollowPlayer();
        }

        private void FollowPlayer()
        {
            if (_playerController != null)
            {
                transform.position = _playerController.transform.position + new Vector3(0, 2f, 0);
            }
        }

        public void ResetCoin()
        {
            if (!Object.HasStateAuthority) return;
            
            _hits.Clear();
            
            OwnerId = NetworkBehaviourId.None;
            _playerController = null;

            float randomX;
            float randomZ;

            do
            {
                randomX = Random.Range(-sceneRadius, sceneRadius);
                randomZ = Random.Range(-sceneRadius, sceneRadius);
            } 
            while ((randomX * randomX + randomZ * randomZ) > sceneRadius * sceneRadius);
            
            transform.position = new Vector3(randomX, 2, randomZ);
        }
    }
}
