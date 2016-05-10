using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Must be on the Scene
/// </summary>
public class ItemManager : MonoBehaviour 
{
    /// <summary>
    /// Both array must have the same size.
    /// arrange these items from the most likely to spawn when you are leading from the less likely
    /// </summary>
    public List<GameObject> itemsLevel1 = new List<GameObject>();
    public List<GameObject> itemsLevel2 = new List<GameObject>();

    public GameObject UpgradeItem(GameObject _item)
    {
        if (itemsLevel1.Contains(_item)) // item is level 1, can be upgraded
            return itemsLevel2[itemsLevel1.IndexOf(_item)];
        return _item; // item is already level 2
    }

    public GameObject PickSpecificItem(string itemName)
    {
        return itemsLevel1.Concat(itemsLevel2).SingleOrDefault(x => x.name == itemName);
    }

    public GameObject PickRandomItem()
    {
        return itemsLevel1[Random.Range(0, itemsLevel1.Count)];
    }

    /// <summary>
    /// is more likely to return a useful item depending of the player position
    /// </summary>
    public GameObject PickNotSoRandomItem()
    {
        // TODO : implement a nice algo
        return null;
    }
}
