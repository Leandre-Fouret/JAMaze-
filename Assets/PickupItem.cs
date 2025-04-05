using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    public Sprite icon; // Pour l'inventaire
    public GameObject itemModel; // Ce qui sera affiché dans la main
    public bool isPickedUp = false; // Ajout d'un booléen pour savoir si l'objet a déjà été pris
}
