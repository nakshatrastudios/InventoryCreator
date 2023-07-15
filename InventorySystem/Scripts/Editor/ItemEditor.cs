using System.Collections.Generic;
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
    List<Item.StatModification> statModifications = new List<Item.StatModification>();
    Item.ItemType itemType;
    ItemDatabase itemDatabase; // Field for selecting an ItemDatabase

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
        itemType = (Item.ItemType)EditorGUILayout.EnumPopup("Item Type", itemType);

        EditorGUILayout.BeginHorizontal();
        itemDatabase = (ItemDatabase)EditorGUILayout.ObjectField("Item Database", itemDatabase, typeof(ItemDatabase), false);
        
        if (GUILayout.Button("+"))
        {
            string baseName = "NewItemDatabase";
            string path = "Assets/InventorySystem/";
            int index = 1;

            // Check if a file with the same name already exists
            while (AssetDatabase.LoadAssetAtPath<ItemDatabase>(path + baseName + index + ".asset") != null)
            {
                index++; // Increment the index if a file with the same name exists
            }

            // Create a new ItemDatabase with a unique name
            ItemDatabase newItemDatabase = ScriptableObject.CreateInstance<ItemDatabase>();
            AssetDatabase.CreateAsset(newItemDatabase, path + baseName + index + ".asset");
            AssetDatabase.SaveAssets();

            itemDatabase = newItemDatabase; // Assign the new ItemDatabase to the itemDatabase field
        }


        EditorGUILayout.EndHorizontal(); // Close the horizontal group

        GUILayout.Label("Stat Modification", EditorStyles.boldLabel);
        for (int i = 0; i < statModifications.Count; i++)
        {
            Item.StatModification statModification = statModifications[i];
            statModification.statName = EditorGUILayout.TextField("Stat Name", statModification.statName);
            statModification.modificationValue = EditorGUILayout.IntField("Modification Value", statModification.modificationValue);
            statModifications[i] = statModification;
        }

        if (GUILayout.Button("Add Stat Modification"))
        {
            statModifications.Add(new Item.StatModification());
        }

        // if (GUILayout.Button("Create Item"))
        // {
        //     Item newItem = ScriptableObject.CreateInstance<Item>();
        //     newItem.itemName = itemName;
        //     newItem.itemID = System.Guid.NewGuid().ToString();
        //     newItem.itemQuantity = itemQuantity;
        //     newItem.itemIcon = itemIcon;
        //     newItem.stackable = stackable;
        //     newItem.itemType = itemType;
        //     newItem.statModifications = statModifications;

        //     // Create a new GameObject
        //     GameObject newGameObject = new GameObject(itemName);

        //     Pickup pickup = newGameObject.AddComponent<Pickup>();
        //     pickup.item = newItem;

        //     SphereCollider sphereCollider = newGameObject.AddComponent<SphereCollider>();
        //     sphereCollider.radius = 2f;
        //     sphereCollider.isTrigger = true;

        //     // Add the new item to the selected ItemDatabase
        //     if (itemDatabase != null)
        //     {
        //         itemDatabase.items.Add(newItem);
        //     }
        //     else
        //     {
        //         Debug.LogError("No ItemDatabase selected!");
        //         return; // Return from the method if no ItemDatabase is selected
        //     }

        //     // Save the new GameObject as a prefab
        //     PrefabUtility.SaveAsPrefabAssetAndConnect(newGameObject, "Assets/InventorySystem/Items/" + itemName + ".prefab", InteractionMode.UserAction);

        //     AssetDatabase.CreateAsset(newItem, "Assets/InventorySystem/ItemScriptableObjects/" + itemName + ".asset");
        //     AssetDatabase.SaveAssets();
        //     EditorUtility.FocusProjectWindow();
        //     Selection.activeObject = newItem;
        // }

        if (GUILayout.Button("Create Item"))
        {
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.itemName = itemName;
            newItem.itemID = System.Guid.NewGuid().ToString();
            newItem.itemQuantity = itemQuantity;
            newItem.itemIcon = itemIcon;
            newItem.stackable = stackable;
            newItem.pickupPrefab = pickupPrefab;
            newItem.pickupPromptPrefab = pickupPromptPrefab; // Assign the pickupPromptPrefab field
            newItem.itemType = itemType;
            newItem.statModifications = statModifications;

            // Save the new item as an asset
            AssetDatabase.CreateAsset(newItem, "Assets/InventorySystem/ItemScriptableObjects/" + itemName + ".asset");
            AssetDatabase.SaveAssets();

            // Instantiate the original prefab
            GameObject newGameObject = GameObject.Instantiate(pickupPrefab);

            Pickup pickup = newGameObject.GetComponent<Pickup>();
            if (pickup == null) // If the Pickup component doesn't exist, add it
            {
                pickup = newGameObject.AddComponent<Pickup>();
            }
            pickup.item = newItem; // Now the item is already saved as an asset

            SphereCollider sphereCollider = newGameObject.GetComponent<SphereCollider>();
            if (sphereCollider == null) // If the SphereCollider component doesn't exist, add it
            {
                sphereCollider = newGameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = 2f;
                sphereCollider.isTrigger = true;
            }

            // Add the new item to the selected ItemDatabase
            if (itemDatabase != null)
            {
                itemDatabase.items.Add(newItem);
            }
            else
            {
                Debug.LogError("No ItemDatabase selected!");
                return; // Return from the method if no ItemDatabase is selected
            }

            // Save the new GameObject as a prefab
            PrefabUtility.SaveAsPrefabAssetAndConnect(newGameObject, "Assets/InventorySystem/Items/" + itemName + ".prefab", InteractionMode.UserAction);

            // Destroy the new GameObject after saving it as a prefab
            DestroyImmediate(newGameObject);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newItem;
        }

    }

    ItemDatabase CreateNewItemDatabase()
    {
        ItemDatabase newItemDatabase = ScriptableObject.CreateInstance<ItemDatabase>();
        AssetDatabase.CreateAsset(newItemDatabase, "Assets/InventorySystem/NewItemDatabase.asset");
        AssetDatabase.SaveAssets();
        return newItemDatabase;
    }
}

