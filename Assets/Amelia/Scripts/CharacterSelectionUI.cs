using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterControl
{
    public class CharacterSelectionUI : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionController characterSelectionController = null;
        [SerializeField] private Button[] characterButtons = null;
        [SerializeField] private TMP_Text characterTypeTxt = null;
        [SerializeField] private TMP_Text characterDescriptionTxt = null;

        private void Update()
        {
            setButtonSelected(characterSelectionController.SelectedIndex);
        }
        private void setButtonSelected(int index)
        {

            for (int i = 0; i < characterButtons.Length; i++)
            {
                if (index == i)
                {
                    characterButtons[i].Select();
                    //characterTypeTxt.text = characterSelectionController.SelectedCharacterInfo.Type.ToString(); 這個地方要先修正資訊
                    //characterDescriptionTxt.text= characterSelectionController.CharacterInfo.Description;
                }
            }
        }

        public void OnCharacterButtonClicked(int index)
        {
            characterSelectionController.SetSelectedIndex(index);
        }
    }

}
