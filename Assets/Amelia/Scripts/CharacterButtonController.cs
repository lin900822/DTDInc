using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterControl
{
    public class CharacterButtonController : MonoBehaviour
    {
        public Button[] btn;
        public CharacterSelectionController characterSelectionController;

        private void Start()
        {
            btn[0].onClick.AddListener(OnBtn0Clicked);
            btn[1].onClick.AddListener(OnBtn1Clicked);
            btn[2].onClick.AddListener(OnBtn2Clicked);
            btn[3].onClick.AddListener(OnBtn3Clicked);
            btn[4].onClick.AddListener(OnBtn4Clicked);
            characterSelectionController =gameObject.GetComponent<CharacterSelectionController>();
        }


        public void OnBtn0Clicked()
        {
            Debug.Log("Yellow");
            characterSelectionController.SetSelectedIndex(0);
        }

        

        public void OnBtn1Clicked()
        {
            //Debug.Log("Red");
            characterSelectionController.SetSelectedIndex(1);
        }

        public void OnBtn2Clicked()
        {
            //Debug.Log("Green");
            characterSelectionController.SetSelectedIndex(2);
        }

        public void OnBtn3Clicked()
        {
            //Debug.Log("Blue");
            characterSelectionController.SetSelectedIndex(3);
        }

        public void OnBtn4Clicked()
        {
            //Debug.Log("Blue");
            characterSelectionController.SetSelectedIndex(4);
        }


    }

}


