using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemID;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool stackable;
    public GameObject pickupPrefab;
    public GameObject pickupPromptPrefab;
    public StatModification statModification;
}

[System.Serializable]
public struct StatModification
{
    public string statName;
    public int modificationValue;
}
