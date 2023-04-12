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

        [SerializeField] private NetworkTransform networkTransform = null;
        [SerializeField] private GameObject effect = null;

        [SerializeField] private AudioSource audioSource = null;

        [SerializeField] private LayerMask hitLayers = default;

        private PlayerController _playerController = null;

        private readonly List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();

        [Networked(OnChanged = nameof(OnPlayerIdChanged))]
        public NetworkBehaviourId OwnerId { get; set; }

        [Networked] public PlayerRef OwnerPlayerRef { get; set; }

        private GameManager _gameManager = null;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameManager.RoundManager.Stage == RoundStage.Ready) return;

            DetectPlayer();

            _playerController = Runner.TryFindBehaviour(OwnerId, out PlayerController obj) ? obj : null;

            if (OwnerPlayerRef != default)
            {
                var playerData = GameApp.Instance.GetPlayerNetworkData(OwnerPlayerRef);

                float weight = 1f;
                float roundRemainTime = GameManager.Instance.RoundManager.TimerRemainingTime;

                if (roundRemainTime <= 30f)
                    weight = 1.25f;
                else if (roundRemainTime <= 25f)
                    weight = 1.5f;
                else if (roundRemainTime <= 20f)
                    weight = 1.75f;
                else if (roundRemainTime <= 15f)
                    weight = 2f;
                else if (roundRemainTime <= 10f)
                    weight = 2.5f;
                else if (roundRemainTime <= 1f)
                    weight = 10f;

                if (GameManager.Instance.RoundManager.Stage == RoundStage.InGame)
                    playerData.KeepCoinTime += Runner.DeltaTime * weight;
            }

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

            Runner.LagCompensation.OverlapSphere(transform.position, detectRadius, Object.InputAuthority, _hits,
                hitLayers, HitOptions.None);

            if (_hits.Count <= 0) return;
            if (!_hits[0].GameObject.TryGetComponent<PlayerController>(out var obj)) return;

            OwnerId = obj.Id;
            OwnerPlayerRef = obj.Object.InputAuthority;

            PlaySound_RPC();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void PlaySound_RPC()
        {
            audioSource.Play();
        }

        public override void Render()
        {
            networkTransform.InterpolationTarget.Rotate(Vector3.up * Time.deltaTime * 50f);

            if (_gameManager.RoundManager.Stage == RoundStage.Ready) return;

            FollowPlayer();
        }

        private void FollowPlayer()
        {
            if (_playerController == null) return;
            if (OwnerId == NetworkBehaviourId.None) return;

            transform.position = _playerController.transform.position + new Vector3(0, 3.5f, 0);
        }

        public void ResetCoin()
        {
            if (!Object.HasStateAuthority) return;

            _hits.Clear();

            OwnerId = NetworkBehaviourId.None;
            OwnerPlayerRef = default;
            _playerController = null;

            float randomX;
            float randomZ;

            bool isGrounded;

            do
            {
                randomX = Random.Range(-sceneRadius, sceneRadius);
                randomZ = Random.Range(-sceneRadius, sceneRadius);

                isGrounded = Physics.Raycast(new Vector3(randomX, 1, randomZ), Vector3.down, Mathf.Infinity);
            } while ((randomX * randomX + randomZ * randomZ) > sceneRadius * sceneRadius || !isGrounded);

            transform.position = new Vector3(randomX, 1.5f, randomZ);
        }

        private static void OnPlayerIdChanged(Changed<Coin> changed)
        {
            changed.Behaviour.effect.SetActive(changed.Behaviour.OwnerId == default);
        }
    }
}