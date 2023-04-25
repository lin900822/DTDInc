using Fusion;
using UnityEngine;

namespace Ability
{
    public class Ability : NetworkBehaviour
    {
        public string AbilityName = "";
        public Sprite AbilityIcon = null;
        public string Description = "";
        public bool CanAim;

        public float PrepareTime;
        public float ExcuteTime;
        public float CleanUpTime;

        protected PlayerController playerController = null;
        protected Transform aimmedTrans = null;

        public virtual void Activate(PlayerController playerController) 
        {
            this.playerController = playerController;
        }

        public virtual void Activate(PlayerController playerController, Transform aimmedTrans)
        {
            this.playerController = playerController;
            this.aimmedTrans = aimmedTrans;
        }

        public virtual void Deactivate()
        {
            // this.playerController = null;
            // this.aimmedTrans = null;
        }

        public virtual void OnPrepare() { }
        public virtual void PrepareUpdate() { }
        public virtual void OnExecute() { }
        public virtual void ExcecuteUpdate() { }
        public virtual void OnCleanUp() { }
        public virtual void CleanUpUpdate() { }
    }
}