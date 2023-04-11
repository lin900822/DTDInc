using System;
using GamePlay;
using UnityEngine;

namespace Player
{
    public class CoinIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject indicator = null;
        
        private void LateUpdate()
        {
            Vector3 coinPos = GameManager.Instance.Coin.transform.position;

            Vector3 indicateVector = coinPos - transform.position;
            indicateVector.y = 0;

            if (indicateVector != Vector3.zero)
            {
                transform.forward = indicateVector;
                indicator.SetActive(true);
            }
            else
            {
                indicator.SetActive(false);
            }
            
        }
    }
}