using System.Collections;
using UnityEngine;
using Fusion;
using Cinemachine;
using Fusion.KCC;

public class PlayerController : NetworkBehaviour
{
    public PlayerInputHandler Input => inputHandler;
    public PlayerAgent Agent => agent;
    public KCC KCC => Agent.KCC;
    public ThirdPersonCamera ThirdPersonCamera => thirdPersonCamera;
    public PlayerAttackHandler AttackHandler => attackHandler;
    public PlayerAbilityHandler AbilityHandler => abilityHandler;
    public PlayerUIHandler UIHandler => uIHandler;

    public Camera PlayerCamera => playerCamera;

    [SerializeField] private PlayerInputHandler inputHandler = null;
    [SerializeField] private PlayerAgent agent = null;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera = null;
    [SerializeField] private PlayerAttackHandler attackHandler = null;
    [SerializeField] private PlayerAbilityHandler abilityHandler = null;
    [SerializeField] private PlayerUIHandler uIHandler = null;

    [SerializeField] private Transform cameraFollow = null;
    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private AudioListener audioListener = null;
    [SerializeField] private GameObject playerUi = null;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            
        }
        else
        {
            playerCamera.enabled = false;
            audioListener.enabled = false;
            playerUi.SetActive(false);
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if(transform.position.y <= -20f)
        {
            KCC.SetPosition(SpawnPointManager.Instance.GetRandomSpawnPoint(Runner.Simulation.Tick).position);
        }
    }
}