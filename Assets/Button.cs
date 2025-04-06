using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Silas"); // Mets ici le nom exact de ta scène de jeu
    }

    public void OpenSettings()
    {
        // À implémenter : ouvrir un panel Settings
        Debug.Log("Settings ouvert");
    }

    public void OpenCredits()
    {
        // À implémenter : ouvrir un panel Credits
        Debug.Log("Crédits ouverts");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jeu quitté");
    }
}
