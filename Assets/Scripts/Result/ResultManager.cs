using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Text winnerTxt = null;

    [SerializeField] private GameObject[] playerModels = null;

    private void Start()
    {
        winnerTxt.text = GameApp.Instance.WinnerData.PlayerName + " has won the game!";

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerModels[i].SetActive(i == GameApp.Instance.WinnerData.SelectedCharacterIndex - 1);
        }

        Invoke("BackToCharacterSelection", 10f);
    }

    private void BackToCharacterSelection()
    {
        GameApp.Instance.ResetAllPlayerData();
        GameApp.Instance.Runner.SetActiveScene("Character Selection");
    }
}
