using Fusion;
using SO;
using UnityEngine;

namespace Player
{
    public class PlayerCameraHandler : NetworkBehaviour
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

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Hit_RPC()
        {
            cameraShake.ShakeCamera(cameraShakeSo.HitShake.x, cameraShakeSo.HitShake.y);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Explosion_RPC()
        {
            cameraShake.ShakeCamera(cameraShakeSo.ExplosionShake.x, cameraShakeSo.ExplosionShake.y);
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void Laser_RPC()
        {
            cameraShake.ShakeCamera(cameraShakeSo.LaserShake.x, cameraShakeSo.LaserShake.y);
        }
    }
}