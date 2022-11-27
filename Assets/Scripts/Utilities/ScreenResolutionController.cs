using UnityEngine;

namespace Utilities
{
    public class ScreenResolutionController : MonoBehaviour
    {
        [SerializeField] Vector2 resolution = Vector2.zero;
        [SerializeField] bool isFullScreen = false;

        private void Start()
        {
            Screen.SetResolution((int)resolution.x, (int)resolution.y, isFullScreen);
        }
    }
}
