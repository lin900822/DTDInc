namespace HappyNerd
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    public class FPSDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Text fpsText = null;

        [SerializeField]
        private float updateFrequency = .3f;

        float fps = 0f;

        private void Start()
        {
            fpsText = GetComponent<Text>();

            InvokeRepeating(nameof(DisplayFPS), 0f, updateFrequency);
        }

        private void Update()
        {
            fps = (1f / Time.deltaTime);
        }

        private void DisplayFPS()
        {
            fpsText.text = fps.ToString("0") + " FPS";
        }
    }

}