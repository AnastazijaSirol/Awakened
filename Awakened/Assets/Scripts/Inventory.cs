using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public Transform itemsParent;
    public GameObject emptySlotPrefab;

    private List<GameObject> items = new List<GameObject>();
    private InventoryUI ui;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        ui = GetComponent<InventoryUI>();
        ui.CreateSlots();
    }

    public void AddItem(GameObject icon)
    {
        items.Add(icon);
        ui.UpdateUI(items);
    }
}