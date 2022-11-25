using UnityEngine;

namespace Lobby
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Panel[] panels = null;

        private void Start()
        {
            SwitchPanel(0);
        }

        public void SwitchPanel(int panelIndex)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (panelIndex == i)
                {
                    panels[i].SetActive(true);
                }
                else
                {
                    panels[i].SetActive(false);
                }
            }
        }

        public void StartLoading()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(false);
            }
        }
    }
}
