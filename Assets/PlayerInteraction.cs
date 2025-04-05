using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float pickupRange = 3f;          // Distance de d�tection
    public LayerMask pickupLayer;           // Couche des objets � ramasser
    public Transform handTransform;         // Transform de la main du joueur pour tenir l'objet
    public GameObject pickupPrompt;         // R�f�rence vers le prompt UI

    private PickupItem itemInSight;         // L'objet actuellement d�tect�
    private GameObject heldItem;            // L'objet actuellement tenu par le joueur

    public float placeDistance = 2f;        // Distance � laquelle l'objet est pos� devant le joueur

    void Update()
    {
        DetectPickupItem();  // D�tecter l'objet � ramasser

        // Ramasser l'objet
        if (itemInSight != null && Input.GetKeyDown(KeyCode.E) && heldItem == null)
        {
            PickupItem item = itemInSight;
            heldItem = Instantiate(item.itemModel, handTransform.position, Quaternion.identity, handTransform);  // Cr�e l'objet dans la main
            heldItem.transform.localPosition = Vector3.zero;  // Positionne l'objet correctement dans la main
            heldItem.transform.localRotation = Quaternion.identity;

            // D�sactiver l'objet dans le monde
            Destroy(item.gameObject);
            itemInSight = null;
            pickupPrompt.SetActive(false);  // Masquer le prompt de ramassage
        }

        // Reposer l'objet
        if (heldItem != null && Input.GetKeyDown(KeyCode.R))
        {
            PlaceItemInWorld();  // Repose l'objet dans le monde
            Destroy(heldItem);  // D�truire l'objet de la main
            heldItem = null;  // Supprimer la r�f�rence de l'objet tenu
        }
    }

    void DetectPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);  // Raycast depuis la cam�ra
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))  // V�rifie les objets dans la port�e
        {
            PickupItem item = hit.collider.GetComponent<PickupItem>();
            if (item != null && heldItem == null)  // S'assurer qu'on n'a pas d�j� un objet en main
            {
                itemInSight = item;
                pickupPrompt.SetActive(true);  // Affiche le prompt de ramassage
                return;
            }
        }

        // Si aucun objet n'est d�tect�
        itemInSight = null;
        pickupPrompt.SetActive(false);  // Masquer le prompt
    }

    void PlaceItemInWorld()
    {
        // Utilise une position directement devant le joueur � une certaine distance
        Vector3 positionToPlace = Camera.main.transform.position + Camera.main.transform.forward * placeDistance;

        // Ajuster la position pour �viter que l'objet soit trop coll� � la cam�ra ou dans un obstacle
        positionToPlace.y = 1.0f;  // Positionner sur le sol, ajuster si n�cessaire selon la hauteur du terrain

        // Instancier l'objet dans le monde � la position calcul�e
        Instantiate(heldItem, positionToPlace, Quaternion.identity);
    }
}
