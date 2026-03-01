using UnityEngine;

public class PlayerData : MonoBehaviour
{
    // Allows script to be directly referenced without variables and saves it across scenes
    #region Singleton

    private static PlayerData instance;
    public static PlayerData Instance
    {
        get => instance;
        private set
        {
            if (instance == null)
            {
                instance = value;
                DontDestroyOnLoad(value);
            }
            else if (instance != value)
            {
                Debug.Log($"{nameof(PlayerData)} intance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public int gameMode = 1;
    public GameObject playerModel;
}
