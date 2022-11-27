using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class SkillBox : NetworkBehaviour
    {
        [SerializeField] private GameObject visual = null;

        [SerializeField] private float sceneRadius = 45f;

        [SerializeField] private float lifeTime = 1f;
        [SerializeField] private float coolDownTime = 1f;

        [SerializeField] private Vector3 detectionBox = Vector3.one;
        [SerializeField] private LayerMask hitLayer = default;

        [SerializeField] private List<string> abilities = new List<string>();

        [Networked(OnChanged = nameof(OnIsActiveChanged))] private NetworkBool IsActive { get; set; }
        [Networked] private TickTimer coolDownTimer { get; set; }

        private GameManager _gameManager = null;
        
        private List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public override void Spawned()
        {
            coolDownTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
        }

        public override void FixedUpdateNetwork()
        {
            Respawn();

            DetectPlayer();
        }

        private void Respawn()
        {
            if (!Object.HasStateAuthority) return;

            if (coolDownTimer.Expired(Runner) && !IsActive)
            {
                IsActive = true;

                float randomX;
                float randomZ;

                do
                {
                    randomX = Random.Range(-sceneRadius, sceneRadius);
                    randomZ = Random.Range(-sceneRadius, sceneRadius);
                } 
                while ((randomX * randomX + randomZ * randomZ) > sceneRadius * sceneRadius);

                transform.position = new Vector3(randomX, transform.position.y, randomZ);
            }
        }

        private void DetectPlayer()
        {
            if (_gameManager.RoundManager.Stage != RoundStage.InGame) return;
            if (!Object.HasStateAuthority) return;
            if (!IsActive) return;

            int hitAmount = Runner.LagCompensation.OverlapBox(transform.position, detectionBox, Quaternion.identity, Object.InputAuthority, hits, hitLayer, HitOptions.IgnoreInputAuthority);

            if (hitAmount > 0)
            {
                if (hits[0].GameObject.TryGetComponent<PlayerController>(out var playerController))
                {
                    Random.InitState(Runner.Simulation.Tick);
                    int randomValue = Random.Range(0, abilities.Count);
                    string abilityName = abilities[randomValue];

                    if (playerController.AbilityHandler.AddAbilityToSlots(abilityName))
                    {
                        IsActive = false;
                        coolDownTimer = TickTimer.CreateFromSeconds(Runner, coolDownTime);
                    }
                }
            }
        }

        private static void OnIsActiveChanged(Changed<SkillBox> changed)
        {
            changed.Behaviour.SetVisible(changed.Behaviour.IsActive);
        }

        private void SetVisible(bool isVisible)
        {
            visual.SetActive(isVisible);
        }
    }
}
