using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private NetworkCharacterControllerPrototype networkCharacterController = null;

    [SerializeField] private GameObject playerCamera = null;

    [SerializeField] private float moveSpeed = 15f;

    // 控制玩家旋轉
    [Networked] private Angle Yaw { get; set; }

    // 保存上一次的Input
    [Networked] private NetworkButtons ButtonsPrevious { get; set; }

    public override void Spawned()
    {
        // 只開啟屬於自己玩家操控的 Camera
        if (Object.HasInputAuthority)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }
    }

    // 1s 60 ticks
    public override void FixedUpdateNetwork()
    {
        // 接收 Input 
        if (GetInput(out NetworkInputData data))
        {
            // 將本次的Input 與 上一次的Input 比對 ，得出被按下去的按鈕狀態
            NetworkButtons buttons = data.buttons;

            NetworkButtons pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;

            // Movement
            Vector3 moveVector = data.movementInput.normalized;
            moveVector = transform.TransformDirection(moveVector);
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            // Rotation
            Yaw += data.Yaw;

            transform.rotation = Quaternion.Euler(0, (float)Yaw, 0);

            // Buttons(Jump)
            if (pressed.IsSet(InputButtons.JUMP))
            {
                networkCharacterController.Jump();
            }
        }
    }
}