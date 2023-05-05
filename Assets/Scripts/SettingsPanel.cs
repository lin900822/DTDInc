using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    public static SettingsPanel Instance { get; private set; }
    
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private TMP_Text volumeValueTxt = null;

    private int volumeValue = 100;
    private int resolutionValue = 0;
    private bool isFullScreen = true;

    private bool previousVisible = true;
    private CursorLockMode previousCursorLockMode = CursorLockMode.None;
    
    private bool isOpen = false;
    private bool IsOpen
    {
        get => isOpen;
        set
        {
            isOpen = value;
            if (isOpen)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;

                previousVisible = Cursor.visible;
                previousCursorLockMode = Cursor.lockState;
                
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                Cursor.visible = previousVisible;
                Cursor.lockState = previousCursorLockMode;
            }
        }
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        IsOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsOpen = !IsOpen;
        }
    }

    public void OnConfirmBtnClicked()
    {
        IsOpen = false;
        
        switch (resolutionValue)
        {
            case 0:
                Screen.SetResolution(1920, 1080, isFullScreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, isFullScreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, isFullScreen);
                break;
            case 3:
                Screen.SetResolution(960, 540, isFullScreen);
                break;
        }
    }

    public void OnExitBtnClicked()
    {
        Application.Quit();
    }

    public void SetVolume(float value)
    {
        volumeValue = (int)(value * 100);
        volumeValueTxt.text = volumeValue.ToString();

        audioMixer.SetFloat("MasterVolume", volumeValue * 0.8f - 80);
    }

    public void SetResolution(int value)
    {
        resolutionValue = value;
    }

    public void SetFullScreen(bool value)
    {
        isFullScreen = value;
    }
}