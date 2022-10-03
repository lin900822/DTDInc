using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private NetworkCharacterControllerPrototype networkCharacterController = null;

    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private GameObject playerUI = null;
    [SerializeField] private Transform cameraPivot = null;
    [SerializeField] private AudioListener audioListener = null;

    [SerializeField] private float moveSpeed = 15f;

    // 控制玩家旋轉
    [Networked] private Angle Yaw { get; set; }
    [Networked] private Angle Pitch { get; set; }

    // 保存上一次的Input
    [Networked] private NetworkButtons ButtonsPrevious { get; set; }

    public override void Spawned()
    {
        // 只開啟屬於自己玩家操控的 Camera
        if (Object.HasInputAuthority)
        {
            playerCamera.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
            playerUI.SetActive(false);
            audioListener.enabled = false;
        }
    }

    // 1s 60 ticks
    public override void FixedUpdateNetwork()
    {
        // 接收 Input 
        if (GetInput(out NetworkInputData data))
        {
            // 將本次的Input 與 上一次的Input 比對 ，得出被按下去的按鈕狀態
            NetworkButtons buttons = data.Buttons;

            NetworkButtons pressed = buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = buttons;

            // Movement
            Vector3 moveVector = data.MovementInput.normalized;
            moveVector = transform.TransformDirection(moveVector);
            networkCharacterController.Move(moveSpeed * moveVector * Runner.DeltaTime);

            // Rotation
            Yaw += data.Yaw;
            Pitch += data.Pitch;

            transform.rotation = Quaternion.Euler(0, (float)Yaw, 0);
            cameraPivot.localRotation = Quaternion.Euler(-(float)Pitch, 0, 0);

            // Buttons(Jump)
            if (pressed.IsSet(InputButtons.JUMP))
            {
                networkCharacterController.Jump();
            }

            // Buttons(Fire)
            if (pressed.IsSet(InputButtons.FIRE))
            {
                GameObject hitObj = GetAimmedObject();

                int index = FloorManager.Instance.GetCubeIndex(hitObj);

                FloorManager.Instance.DestroyOneCube_RPC(index);
            }
        }
    }

    // 打一條射線，如果有打到物體，回傳該物體(GameObject)
    private GameObject GetAimmedObject()
    {
        GameObject hitObj = null;

        if (Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out RaycastHit hit, Mathf.Infinity))
        {
            hitObj = hit.transform.gameObject;
        }

        return hitObj;
    } 
}