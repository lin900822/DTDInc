using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInputHandler inputHandler = null;

    [SerializeField]
    private PlayerModel playerModel = null;

    private void Update()
    {
        playerModel.playerRigidbody.velocity = new Vector3(inputHandler.movementInput.x, 0, inputHandler.movementInput.z);
    }
}
