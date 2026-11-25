using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> _itemPrefabs;
    public Dictionary<int, GameObject> _itemDictionary;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < _itemPrefabs.Count; i++)
        {
            if (_itemPrefabs[i] != null)
            {
                _itemPrefabs[i].ID = i + 1;
            }
        }

        foreach (Item item in _itemPrefabs)
        {
            _itemDictionary[item.ID] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        _itemDictionary.TryGetValue(itemID, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary");
        }
        return prefab;
    }
}
