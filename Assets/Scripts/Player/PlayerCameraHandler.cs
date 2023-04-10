using SO;
using UnityEngine;

namespace Player
{
    public class PlayerCameraHandler : MonoBehaviour
    {
        [SerializeField] private Animator cameraAnimator = null;

        [SerializeField] private CameraShake cameraShake = null;

        [SerializeField] private CameraShakeSO cameraShakeSo = null;

        public void SetWalkingAnimation(bool isWalking)
        {
            cameraAnimator.SetBool("isWalking", isWalking);
        }

        public void TriggerLandAnimation()
        {
            cameraAnimator.SetTrigger("onLand");
            cameraShake.ShakeCamera(cameraShakeSo.LandShake.x, cameraShakeSo.LandShake.y);
        }
    }
}