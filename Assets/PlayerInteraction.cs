using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float pickupRange = 3f;
    public LayerMask pickupLayer;
    public Transform handTransform;
    public GameObject pickupPrompt;

    public TextMeshProUGUI doorPromptText; // Texte � afficher

    private PickupItem itemInSight;
    private GameObject heldItem;

    public float placeDistance = 2f;

    void Update()
    {
        DetectPickupItem();
        DetectDoorInFront();

        if (itemInSight != null && Input.GetKeyDown(KeyCode.E) && heldItem == null)
        {
            PickupItem item = itemInSight;
            heldItem = Instantiate(item.itemModel, handTransform.position, Quaternion.identity, handTransform);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
            Destroy(item.gameObject);
            itemInSight = null;
            pickupPrompt.SetActive(false);
        }

        if (heldItem != null && Input.GetKeyDown(KeyCode.R))
        {
            PlaceItemInWorld();
            Destroy(heldItem);
            heldItem = null;
        }

        if (heldItem != null && Input.GetKeyDown(KeyCode.F))
        {
            TryUseKeyOnDoor();
        }
    }

    void DetectPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            PickupItem item = hit.collider.GetComponent<PickupItem>();
            if (item != null && heldItem == null)
            {
                itemInSight = item;
                pickupPrompt.SetActive(true);
                return;
            }
        }

        itemInSight = null;
        pickupPrompt.SetActive(false);
    }

    void PlaceItemInWorld()
    {
        Vector3 positionToPlace = Camera.main.transform.position + Camera.main.transform.forward * placeDistance;
        positionToPlace.y = 1.0f;
        Instantiate(heldItem, positionToPlace, Quaternion.identity);
    }

    void TryUseKeyOnDoor()
    {
        PickupItem heldItemScript = heldItem.GetComponent<PickupItem>();
        if (heldItemScript == null || !heldItemScript.isKey)
            return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                if (heldItemScript.keyID == door.doorID)
                {
                    door.TryOpen(heldItemScript.keyID);
                    Destroy(heldItem);
                    heldItem = null;
                    doorPromptText.gameObject.SetActive(false); // D�sactiver le texte apr�s ouverture
                }
                else
                {
                    Debug.Log("Mauvaise cl� !");
                }
            }
        }
    }

    void DetectDoorInFront()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Door door = hit.collider.GetComponent<Door>();
            if (door != null)
            {
                Debug.Log("Porte d�tect�e : " + door.name);  // Log pour v�rifier que la porte est bien d�tect�e
                doorPromptText.gameObject.SetActive(true); // Afficher le texte

                PickupItem heldItemScript = heldItem != null ? heldItem.GetComponent<PickupItem>() : null;

                // Si on a un objet en main, v�rifier s'il s'agit d'une cl�
                if (heldItemScript != null)
                {
                    Debug.Log("Objet en main : " + heldItemScript.itemName);  // Log pour v�rifier l'objet en main

                    if (heldItemScript.isKey)
                    {
                        Debug.Log("C'est une cl� !");  // Log si l'objet est une cl�

                        if (heldItemScript.keyID == door.doorID)
                        {
                            Debug.Log("La cl� correspond � la porte !");  // Log pour v�rifier si la cl� correspond � la porte
                            doorPromptText.text = "Appuyez sur F pour ouvrir la porte";
                        }
                        else
                        {
                            Debug.Log("La cl� ne correspond pas � la porte");  // Log pour v�rifier que la cl� ne correspond pas
                            doorPromptText.text = "Il vous faut la bonne cl� pour ouvrir la porte";
                        }
                    }
                    else
                    {
                        Debug.Log("L'objet en main n'est pas une cl�");  // Log si l'objet en main n'est pas une cl�
                        doorPromptText.text = "Il vous faut la bonne cl� pour ouvrir la porte";
                    }
                }
                else
                {
                    Debug.Log("Aucun objet en main");  // Log si aucun objet n'est en main
                    doorPromptText.text = "Il vous faut la bonne cl� pour ouvrir la porte";
                }

                return;
            }
        }

        doorPromptText.gameObject.SetActive(false); // Cacher le texte si pas de porte
    }
}
