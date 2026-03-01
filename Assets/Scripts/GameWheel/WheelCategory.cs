using TMPro;
using UnityEngine;

public class WheelCategory : MonoBehaviour
{
    [Tooltip("Scene index of the game to be chosen")]
    public int gameId = 0;
    [Tooltip("Text to display on the wheel")]
    public string gameName = "Dog stuff";
    [Tooltip("Text object that displays the game name")]
    public TMP_Text displayNameText;

    public void InitializeCategory(int id, string text)
    {
        gameId = id;
        gameName = text;
        displayNameText.text = gameName;
    }
}
