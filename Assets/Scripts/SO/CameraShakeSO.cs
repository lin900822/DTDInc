using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Camera Shake Data", menuName = "ScriptableObjects/Camera Shake Data", order = 1)]
    public class CameraShakeSO : ScriptableObject
    {
        public Vector2 LandShake = new Vector2(0.015f, 0.05f);

        public Vector2 HitShake = new Vector2(0.01f, 0.2f);
        
        public Vector2 ExplosionShake = new Vector2(0.012f, 0.4f);
        
        public Vector2 LaserShake = new Vector2(0.012f, 0.2f);
    }
}
