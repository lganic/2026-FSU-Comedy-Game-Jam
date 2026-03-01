using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeFilter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerData.Instance)
        {
            FilterFor(PlayerData.Instance.gameMode);
        }
        else
        {
            FilterFor(4);
        }
    }

    void Traverse(Transform t, int gamemode)
    {
        GameObject go = t.gameObject;

        if (gamemode != 1)
        {
            if (go.name.StartsWith("GARBAGE"))
            {
                Destroy(go);
                return;
            }
        }

        if (gamemode != 2)
        {
            if (go.name.StartsWith("CAT"))
            {
                Destroy(go);
                return;
            }
        }

        if (gamemode != 3)
        {
            if (go.TryGetComponent(out ResourceTarget comp))
            {
                Destroy(comp);

                if (go.TryGetComponent(out AutoRedHighlight arh))
                {
                    Destroy(arh);
                }
            }
        }

        if (gamemode != 4)
        {
            if (go.TryGetComponent(out AddToInventory ati))
            {
                Destroy(go);
                return;
            }
        }

        foreach (Transform child in t)
            Traverse(child, gamemode);
    }


    public void FilterFor(int gamemode)
    {
        foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            Traverse(root.transform, gamemode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
