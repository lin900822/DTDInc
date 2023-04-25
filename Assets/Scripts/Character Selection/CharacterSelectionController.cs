using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Character_Selection
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [SerializeField] private GameObject[] virtualCameras = new GameObject[5];

        [SerializeField] private Button[] buttons = new Button[5];

        private int selectedIndex = 0;

        private void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var i1 = i;
                buttons[i].onClick.AddListener(() => { OnBtnClicked(i1); });
            }
        }

        public void SetSelectedIndex(int value)
        {
            selectedIndex = value;

            for (int i = 0; i < virtualCameras.Length; i++)
            {
                virtualCameras[i].SetActive(i == selectedIndex);
            }
        }

        private void OnBtnClicked(int index)
        {
            SetSelectedIndex(index);
        }
    }
}