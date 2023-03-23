using UnityEngine;

namespace Player
{
    public class PlayerCameraHandler : MonoBehaviour
    {
        [SerializeField] private Animator cameraAnimator = null;

        public void SetWalking(bool isWalking)
        {
            cameraAnimator.SetBool("isWalking", isWalking);
        }

        public void PlayLandAnimation()
        {
            cameraAnimator.SetTrigger("onLand");
        }
    }
}