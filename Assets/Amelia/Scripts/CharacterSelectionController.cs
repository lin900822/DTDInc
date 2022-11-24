using System.Collections;
using System.Collections.Generic;
using CharacterSelection;
using UnityEngine;
using System;

namespace CharacterControl
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public event Action<int> OnSelectedIndexChange = null;
        public CharacterInfos SelectedCharacterInfos => CharacterInfos[SelectedIndex];
        public CharacterInfos[] CharacterInfos => characterInfos;
        public int SelectedIndex => selectedIndex;

        [SerializeField] private CharacterInfos[] characterInfos = null;


        private int selectedIndex=-1;

        private void Start()
        {
            selectedIndex = 0;
        }

        public void SetSelectedIndex(int index)
        {
            if (index!=selectedIndex)
            {
                selectedIndex = Mathf.Clamp(index, 0, characterInfos.Length - 1); //why?
                OnSelectedIndexChange?.Invoke(selectedIndex);
            }
        }

        private void Update()

        {
            //這邊要再改成監聽按鈕方向
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetSelectedIndex(selectedIndex + 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SetSelectedIndex(selectedIndex - 1);
            }
        }
    }

}
