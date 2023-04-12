using System;
using DG.Tweening;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GamePlay
{
    public class GameUIManager : NetworkBehaviour
    {
        [SerializeField] private CanvasGroup systemMessageCanvasGroup = null;
        [SerializeField] private Text systemMessageTxt = null;
        [SerializeField] private TMP_Text timerText = null;

        [SerializeField] private Animator countDownAnimator = null;

        private float duration = 2f;
        private bool hasFaded = false;
        
        private Tween _messageTween;

        private void Update()
        {
            if (duration <= 0)
            {
                if (!hasFaded)
                {
                    _messageTween.Kill();
                    _messageTween = systemMessageCanvasGroup.DOFade(0f, .25f);
                    hasFaded = true;
                }
            }
            else
            {
                duration -= Time.deltaTime;
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void ShowMessage_Rpc(string msg)
        {
            systemMessageCanvasGroup.alpha = 1f;
            systemMessageTxt.text = msg;

            duration = 2f;
            hasFaded = false;
        }

        public void UpdateTimer(float secondsLeft)
        {
            var minutes = Mathf.Floor(secondsLeft / 60f);
            var seconds = Mathf.Floor(secondsLeft % 60f);

            timerText.text = secondsLeft > 0 ? $"{minutes:00} : {seconds:00}" : $"00 : 00";
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void StartCountDown_Rpc()
        {
            countDownAnimator.SetTrigger("Start");
        }
    }
}