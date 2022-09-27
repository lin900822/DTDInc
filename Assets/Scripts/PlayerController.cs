using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private NetworkCharacterControllerPrototype networkCharacterController = null;

    [SerializeField]
    private float moveSpeed = 15f;

    //[Networked]
    //public NetworkButtons ButtonsPrevious { get; set; }

    // 1s 60 ticks
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //NetworkButtons buttons = data.buttons;
            //var pressed = buttons.GetPressed(ButtonsPrevious);
            //ButtonsPrevious = buttons;

            Vector3 moveVector = data.movementInput.normalized;
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            //if (pressed.IsSet(InputButtons.JUMP))
            //{
            //    networkCharacterController.Jump();
            //}
        }
    }
}