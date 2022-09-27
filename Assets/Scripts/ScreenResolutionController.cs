using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionController : MonoBehaviour
{
    [SerializeField] Vector2 resolution = Vector2.zero;
    [SerializeField] bool isFullScreen = false;

    private void Start()
    {
        Screen.SetResolution((int)resolution.x, (int)resolution.y, isFullScreen);
    }
}
