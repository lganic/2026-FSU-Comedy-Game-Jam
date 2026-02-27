using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public GameObject BubblePrefab;

    public void AddBubble(GameObject object_to_add_bubble, List<string> text_dialogs)
    {
        GameObject newChild = Instantiate(BubblePrefab, object_to_add_bubble.transform);
        newChild.transform.localPosition = new Vector3(0, 2, 0);

        BubbleController bc = newChild.GetComponent<BubbleController>();

        bc.Dialogs = text_dialogs;
    }

}
