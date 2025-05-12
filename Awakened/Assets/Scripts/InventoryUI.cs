using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public int slotCount = 5;
    private List<Image> slotImages = new List<Image>();

    public void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = new GameObject("Slot" + i, typeof(Image));
            slot.transform.SetParent(transform);
            var img = slot.GetComponent<Image>();
            img.color = Color.clear;
            slotImages.Add(img);
        }
    }

    public void UpdateUI(List<GameObject> items)
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < items.Count)
            {
                slotImages[i].sprite = items[i].GetComponent<Image>().sprite;
                slotImages[i].color = Color.white;
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = Color.clear;
            }
        }
    }
}