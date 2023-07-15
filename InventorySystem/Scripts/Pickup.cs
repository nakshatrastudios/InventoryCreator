using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    private bool inRange = false;
    private GameObject player = null;
    private GameObject uiPromptInstance = null;

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
            player = other.gameObject;
            uiPromptInstance = Instantiate(item.pickupPromptPrefab, Vector3.zero, Quaternion.identity);
            uiPromptInstance.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
            player = null;
            Destroy(uiPromptInstance);
            uiPromptInstance = null;
        }
    }

    void PickupItem()
    {
        player.GetComponent<InventoryComponent>().AddItem(item);
        Destroy(gameObject);
        if (uiPromptInstance != null)
        {
            Destroy(uiPromptInstance);
        }
    }
}
