using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl
{
    public class CharacterSelectionCameraView:MonoBehaviour
    {
        [SerializeField] private CharacterSelectionController characterSelectionController = null;
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras = null;

        private void Awake()
        {
            characterSelectionController.OnSelectedIndexChange += SetSelectedCamera;
        }
        private void OnDestroy()
        {
            characterSelectionController.OnSelectedIndexChange -= SetSelectedCamera;
        }

        private void SetSelectedCamera(int index)
        {
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                if (index==i)
                {
                    virtualCameras[i].enabled = true;
                }
                else
                {
                    virtualCameras[i].enabled = false;
                }
            }
        }
    }
}
