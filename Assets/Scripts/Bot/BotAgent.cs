using Fusion.KCC;
using GamePlay;
using HappyNerd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAgent : Agent
{
    [SerializeField]
    private Vector3 _jumpImpulse = new Vector3(0f, 6f, 0f);

    private Coin coin = null;

    [HideInInspector] public bool InputBlocked = false;

    private void Start()
    {
        coin = GameManager.Instance.Coin;
    }

    protected override void OnSpawned()
    {
        gameObject.SetActive(Runner.IsSinglePlayer);
    }

    protected override void ProcessEarlyFixedInput()
    {
        // Look Rotation Input

        if (Vector3.Distance(coin.transform.position, transform.position) >= 2f)
        {
            var lookVector = coin.transform.position - transform.position;

            var angle = Vector3.Angle(Vector3.forward, lookVector);
            angle = lookVector.x < 0 ? -angle : angle;

            KCC.SetLookRotation(0, angle);
        }

        // Movement Input

        Vector3 inputDirection = KCC.FixedData.TransformRotation * new Vector3(0, 0, InputBlocked ? 0 : 1);

        KCC.SetInputDirection(inputDirection);

        // Jump
        if (KCC.FixedData.IsGrounded && !Physics.Raycast(transform.position + transform.TransformDirection(Vector3.forward), Vector3.down, 5f) && !InputBlocked)
        {
            Quaternion jumpRotation = KCC.FixedData.TransformRotation;

            if (inputDirection.IsAlmostZero() == false)
            {
                jumpRotation = Quaternion.LookRotation(inputDirection);
            }

            KCC.Jump(jumpRotation * _jumpImpulse);
        }
        
    }

    protected override void ProcessLateFixedInput()
    {
        
    }

    protected override void ProcessRenderInput()
    {
        
    }

    protected override void OnFixedUpdate()
    {
        if (transform.position.y <= -20f)
        {
            KCC.SetPosition(SpawnPointManager.Instance.GetRandomSpawnPoint(Runner.Simulation.Tick).position);
        }
    }
}
