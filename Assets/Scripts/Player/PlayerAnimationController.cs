using Fusion;
using Fusion.KCC;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator = null;
        [SerializeField] private NetworkMecanimAnimator networkAnimator = null;

        private bool _isWalking = false;
        private bool _isAir = false;
        private bool wasAirLastFrame = false;

        private readonly int _xInput = Animator.StringToHash("XInput");
        private readonly int _yInput = Animator.StringToHash("YInput");
        private readonly int _isAirHash = Animator.StringToHash("isAir");
        private readonly int _isWalkingHash = Animator.StringToHash("isWalking");

        public void ProcessInput(PlayerController playerController)
        {
            playerAnimator.SetFloat(_xInput, playerController.Input.FixedInput.MoveDirection.x);
            playerAnimator.SetFloat(_yInput, playerController.Input.FixedInput.MoveDirection.y);

            _isAir = !playerController.KCC.Data.IsGrounded;
            wasAirLastFrame = !playerController.KCC.Data.WasGrounded;
            playerAnimator.SetBool(_isAirHash, _isAir);

            _isWalking = playerController.KCC.Data.RealVelocity.magnitude >= .1f && !_isAir;
            playerAnimator.SetBool(_isWalkingHash, _isWalking);


            if (playerController.Input.WasPressed(InputButtons.Fire))
            {
                networkAnimator.SetTrigger("Attack");
            }
            else if (playerController.Input.WasPressed(InputButtons.UseAbility))
            {
                networkAnimator.SetTrigger("UseAbility");
            }

            if (playerController.HasInputAuthority && playerController.Runner.IsForward)
            {
                playerController.CameraHandler.SetWalkingAnimation(_isWalking);

                if (wasAirLastFrame && !_isAir)
                {
                    playerController.CameraHandler.TriggerLandAnimation();
                }
            }
        }
    }
}