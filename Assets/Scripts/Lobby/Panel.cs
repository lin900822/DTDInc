using UnityEngine;

namespace Lobby
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] protected MenuManager menuManager = null;

        [SerializeField] protected CanvasGroup canvasGroup = null;

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}
