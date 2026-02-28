using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Transform barRoot;
    [SerializeField] private Sprite SockSprite;

    public float spacing = 40;

    // The original Image under barRoot (used as the template)
    private Image template;

    // Keep refs so you can clear/update later
    private readonly List<Image> segments = new();

    private readonly List<Image> inventory_images = new();

    void Awake()
    {
        // Find the first Image under barRoot (or assign it directly if you prefer)
        template = barRoot.GetComponentInChildren<Image>(includeInactive: true);

        if (template == null)
        {
            Debug.LogError("No Image found under barRoot.");
            return;
        }

        // "Remove" it from the bar visually, but keep it as the template
        template.gameObject.SetActive(false);

        BuildBar(4);
        SetInventory(2);
    }

    public int getNumInInvetory()
    {
        return inventory_images.Count;
    }

    private float getImagePos(int total, int index)
    {
        float b = -(total - 1) * spacing / 2;

        return b + index * spacing;
    }

    public void SetInventory(int number)
    {

        foreach (var img in inventory_images)
        {
            if (img) Destroy(img.gameObject);
        }
        inventory_images.Clear();

        for (int i = 0; i < number; i++)
        {
            GameObject imgObj = new GameObject();
            imgObj.transform.SetParent(barRoot);

            Image img = imgObj.AddComponent<Image>();

            img.sprite = SockSprite;

            img.preserveAspect = true;

            imgObj.transform.localPosition = new Vector3(getImagePos(segments.Count, i), 0, 0);

            inventory_images.Add(img);
        }

    }

    public bool CanAddToInventory()
    {
        return segments.Count != inventory_images.Count;
    }

    public void Add1ToInventory()
    {
        if (!CanAddToInventory()) return;

        int current_number = inventory_images.Count;

        SetInventory(current_number + 1);
    }

    public void BuildBar(int count)
    {
        ClearBar();

        for (int i = 0; i < count; i++)
        {
            // Instantiate a copy under the same parent (keeps RectTransform hierarchy)
            var copyGO = Instantiate(template.gameObject, barRoot);
            copyGO.SetActive(true);

            var img = copyGO.GetComponent<Image>();
            segments.Add(img);

            img.transform.localPosition = new Vector3(getImagePos(count, i), 0, 0);
        }
    }

    public void ClearBar()
    {
        foreach (var img in segments)
        {
            if (img) Destroy(img.gameObject);
        }
        segments.Clear();
    }
}