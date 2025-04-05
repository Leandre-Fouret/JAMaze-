using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float pickupRange = 3f;          // Distance de détection
    public LayerMask pickupLayer;           // Couche des objets à ramasser
    public Transform handTransform;         // Transform de la main du joueur pour tenir l'objet
    public GameObject pickupPrompt;         // Référence vers le prompt UI

    private PickupItem itemInSight;         // L'objet actuellement détecté
    private GameObject heldItem;            // L'objet actuellement tenu par le joueur

    public float placeDistance = 2f;        // Distance à laquelle l'objet est posé devant le joueur

    void Update()
    {
        DetectPickupItem();  // Détecter l'objet à ramasser

        // Ramasser l'objet
        if (itemInSight != null && Input.GetKeyDown(KeyCode.E) && heldItem == null)
        {
            PickupItem item = itemInSight;
            heldItem = Instantiate(item.itemModel, handTransform.position, Quaternion.identity, handTransform);  // Crée l'objet dans la main
            heldItem.transform.localPosition = Vector3.zero;  // Positionne l'objet correctement dans la main
            heldItem.transform.localRotation = Quaternion.identity;

            // Désactiver l'objet dans le monde
            Destroy(item.gameObject);
            itemInSight = null;
            pickupPrompt.SetActive(false);  // Masquer le prompt de ramassage
        }

        // Reposer l'objet
        if (heldItem != null && Input.GetKeyDown(KeyCode.R))
        {
            PlaceItemInWorld();  // Repose l'objet dans le monde
            Destroy(heldItem);  // Détruire l'objet de la main
            heldItem = null;  // Supprimer la référence de l'objet tenu
        }
    }

    void DetectPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);  // Raycast depuis la caméra
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))  // Vérifie les objets dans la portée
        {
            PickupItem item = hit.collider.GetComponent<PickupItem>();
            if (item != null && heldItem == null)  // S'assurer qu'on n'a pas déjà un objet en main
            {
                itemInSight = item;
                pickupPrompt.SetActive(true);  // Affiche le prompt de ramassage
                return;
            }
        }

        // Si aucun objet n'est détecté
        itemInSight = null;
        pickupPrompt.SetActive(false);  // Masquer le prompt
    }

    void PlaceItemInWorld()
    {
        // Utilise une position directement devant le joueur à une certaine distance
        Vector3 positionToPlace = Camera.main.transform.position + Camera.main.transform.forward * placeDistance;

        // Ajuster la position pour éviter que l'objet soit trop collé à la caméra ou dans un obstacle
        positionToPlace.y = 1.0f;  // Positionner sur le sol, ajuster si nécessaire selon la hauteur du terrain

        // Instancier l'objet dans le monde à la position calculée
        Instantiate(heldItem, positionToPlace, Quaternion.identity);
    }
}
