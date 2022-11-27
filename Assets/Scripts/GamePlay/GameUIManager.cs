using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GamePlay
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup systemMessageCanvasGroup = null;
        [SerializeField] private TMP_Text systemMessageTxt = null;
        
        private Tween _messageTween;
        
        public void ShowMessage(string msg)
        {
            _messageTween.Kill();
            systemMessageCanvasGroup.alpha = 1f;
            systemMessageTxt.text = msg;
            
            _messageTween = systemMessageCanvasGroup.DOFade(0f, 2f);
        }
    }
}