using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Story
{
    public class StoryPlayer : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer = null;
        [SerializeField] private GameObject pressAnyKeyToStartObj = null;

        [SerializeField] private VideoClip firstVideo = null;
        [SerializeField] private VideoClip repeatVideo = null;

        private bool hasPlayedFirstVideo = false;

        private void Start()
        {
            videoPlayer.clip = firstVideo;
            videoPlayer.Play();
            videoPlayer.isLooping = false;
            hasPlayedFirstVideo = false;
            pressAnyKeyToStartObj.SetActive(false);
        }

        private void Update()
        {
            if (!videoPlayer.isPlaying && !hasPlayedFirstVideo && Time.time > 5f)
            {
                hasPlayedFirstVideo = true;
                videoPlayer.clip = repeatVideo;
                videoPlayer.Play();
                videoPlayer.isLooping = true;
                pressAnyKeyToStartObj.SetActive(true);
            }

            if (Input.anyKeyDown)
            {
                if (hasPlayedFirstVideo)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    hasPlayedFirstVideo = true;
                    videoPlayer.clip = repeatVideo;
                    videoPlayer.Play();
                    videoPlayer.isLooping = true;
                    pressAnyKeyToStartObj.SetActive(true);
                }
            }
        }
    }
}