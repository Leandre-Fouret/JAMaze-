using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    public Sprite icon; // Pour l'inventaire
    public GameObject itemModel; // Ce qui sera affich� dans la main
    public bool isPickedUp = false; // Ajout d'un bool�en pour savoir si l'objet a d�j� �t� pris
}
