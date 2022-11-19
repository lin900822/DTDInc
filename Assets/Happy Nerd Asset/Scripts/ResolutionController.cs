using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField]
    private KeyCode hotKey = KeyCode.Escape;

    [SerializeField]
    private Resolution resolution = Resolution.R1080p;

    [SerializeField]
    private bool isFullScreen = false;

    [SerializeField]
    private GameObject resolutionControllerPanel = null;

    private void Start()
    {
        SetResolution((int)resolution);
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            TriggerPanel();
        }
    }

    public void TriggerPanel()
    {
        resolutionControllerPanel.SetActive(!resolutionControllerPanel.activeSelf);
    }

    public void SetResolution(int value)
    {
        switch ((Resolution)value)
        {
            case Resolution.R2160p:
                Screen.SetResolution(3840, 2160, isFullScreen);
                break;
            case Resolution.R1440p:
                Screen.SetResolution(2560, 1440, isFullScreen);
                break;
            case Resolution.R1080p:
                Screen.SetResolution(1920, 1080, isFullScreen);
                break;
            case Resolution.R720p:
                Screen.SetResolution(1280, 720, isFullScreen);
                break;
            case Resolution.R540p:
                Screen.SetResolution(960, 540, isFullScreen);
                break;
        }
    }

    public void SetFullScreen(bool value)
    {
        isFullScreen = value;
        Screen.fullScreen = isFullScreen;
    }

    public enum Resolution
    {
        R2160p,
        R1440p,
        R1080p,
        R720p,
        R540p
    }
}
