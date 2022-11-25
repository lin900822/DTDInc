using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image = null;

        [SerializeField] private Sprite hoveredSprite = null;
        [SerializeField] private Sprite unhoveredSprite = null;

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.sprite = hoveredSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.sprite = unhoveredSprite;
        }
    }
}
