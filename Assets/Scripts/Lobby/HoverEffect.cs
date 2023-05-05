using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform rectTransform = null;

        [SerializeField] private Vector2 hoverScale = Vector2.one;
        
        private Vector2 targetScale = Vector2.one;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            targetScale = hoverScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            targetScale = Vector2.one;
        }

        private void Update()
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * 10);
        }
    }
}
