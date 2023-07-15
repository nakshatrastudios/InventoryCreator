using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorWindow
{
    string itemName = "New Item";
    int itemQuantity = 0;
    Sprite itemIcon;
    bool stackable = false;
    GameObject pickupPrefab;
    GameObject pickupPromptPrefab;
    StatModification statModification;

    [MenuItem("Inventory System/Create Item")]
    public static void ShowWindow()
    {
        GetWindow<ItemEditor>("Create Item");
    }

    void OnGUI()
    {
        GUILayout.Label("Create a new item", EditorStyles.boldLabel);

        itemName = EditorGUILayout.TextField("Item Name", itemName);
        itemQuantity = EditorGUILayout.IntField("Item Quantity", itemQuantity);
        itemIcon = (Sprite)EditorGUILayout.ObjectField("Item Icon", itemIcon, typeof(Sprite), false);
        stackable = EditorGUILayout.Toggle("Stackable", stackable);
        pickupPrefab = (GameObject)EditorGUILayout.ObjectField("Pickup Prefab", pickupPrefab, typeof(GameObject), false);
        pickupPromptPrefab = (GameObject)EditorGUILayout.ObjectField("Pickup Prompt Prefab", pickupPromptPrefab, typeof(GameObject), false);

        GUILayout.Label("Stat Modification", EditorStyles.boldLabel);
        statModification.statName = EditorGUILayout.TextField("Stat Name", statModification.statName);
        statModification.modificationValue = EditorGUILayout.IntField("Modification Value", statModification.modificationValue);

        if (GUILayout.Button("Create Item"))
        {
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.itemName = itemName;
            newItem.itemID = System.Guid.NewGuid().ToString();
            newItem.itemQuantity = itemQuantity;
            newItem.itemIcon = itemIcon;
            newItem.stackable = stackable;
            newItem.pickupPrefab = pickupPrefab;
            newItem.pickupPromptPrefab = pickupPromptPrefab;
            newItem.statModification = statModification;

            Pickup pickup = newItem.pickupPrefab.AddComponent<Pickup>();
            pickup.item = newItem;
            SphereCollider sphereCollider = newItem.pickupPrefab.AddComponent<SphereCollider>();
            sphereCollider.radius = 2f;
            sphereCollider.isTrigger = true;

            AssetDatabase.CreateAsset(newItem, "Assets/InventorySystem/ItemScriptableObjects/" + itemName + ".asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = newItem;
        }
    }
}
