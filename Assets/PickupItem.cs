using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public GameObject itemModel;
    public bool isPickedUp = false;

    // Nouveau : pour gérer les clés
    public bool isKey = false;
    public string keyID; // Un ID unique pour savoir quelle porte elle ouvre
}