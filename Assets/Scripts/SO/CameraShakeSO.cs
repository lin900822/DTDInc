using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Camera Shake Data", menuName = "ScriptableObjects/Camera Shake Data", order = 1)]
    public class CameraShakeSO : ScriptableObject
    {
        public Vector2 LandShake = new Vector2(0.015f, 0.05f);
    }
}
