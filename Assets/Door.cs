using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorID;
    public bool isOpen = false;

    public void TryOpen(string keyID)
    {
        if (keyID == doorID)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("Ce n'est pas la bonne cl� !");
        }
    }

    private void OpenDoor()
    {
        if (isOpen) return;

        Debug.Log("Porte ouverte !");
        isOpen = true;
        // Tu peux ici jouer une animation, d�sactiver le collider, etc.
        // Exemple :
        // GetComponent<Animator>().SetTrigger("Open");
        // GetComponent<Collider>().enabled = false;
        gameObject.SetActive(false); // Ou juste d�sactiver
    }
}
