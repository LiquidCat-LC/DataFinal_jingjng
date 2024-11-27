using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += 1;
        }
        else
        {
            inventory.Add(itemName, 1);
        }

        Debug.Log($"add item {itemName} => total: {inventory[itemName]}");
        if (itemName == "FireStorm")
        {
            UIManager.Instance.UpdateScrollNum(1);
        }
    }

    public void AddItem(string itemName, int amount)
    {
        // ...
    }

    public void UseItem(string itemName)
    {
        if (inventory.ContainsKey(itemName) && inventory[itemName] > 0)
        {
            inventory[itemName]--;
            Debug.Log($"remove {itemName} remaining: {inventory[itemName]}");
            if (itemName == "FireStorm")
            {
                UIManager.Instance.UpdateScrollNum(-1);
            }

            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
        }
        else
        {
            Debug.LogWarning($"no item {itemName}");
        }
    }

    public void UseItem(string itemName, int amount)
    {
        // ...
    }

    public int numberOfItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        else
        {
            return 0;
        }
    }

    public void ShowInventory()
    {
        Debug.Log("Inventory ...");
        foreach (KeyValuePair<string, int> item in inventory)
        {
            Debug.Log($"{item.Key}: {item.Value}");
        }
    }
}
