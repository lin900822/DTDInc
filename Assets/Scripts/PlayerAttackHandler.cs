using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : NetworkBehaviour
{
    [SerializeField] private PlayerController playerController = null;

    public void ProcessInput()
    {
        if (playerController.Input.WasPressed(InputButtons.Fire))
        {
            DestroyCube();
        }
    }

    private void DestroyCube()
    {
        if(Physics.Raycast(playerController.PlayerCamera.transform.position, 
            playerController.PlayerCamera.transform.TransformDirection(Vector3.forward),
            out RaycastHit hit,
            Mathf.Infinity,
            - 1))
        {
            var floorManager = FloorManager.Instance;
            int hitCubeIndex = floorManager.GetCubeIndex(hit.collider.gameObject);
            floorManager.DestroyOneCube_RPC(hitCubeIndex);
        }
    }
}
