using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Silas"); // Mets ici le nom exact de ta sc�ne de jeu
    }

    public void OpenSettings()
    {
        // � impl�menter : ouvrir un panel Settings
        Debug.Log("Settings ouvert");
    }

    public void OpenCredits()
    {
        // � impl�menter : ouvrir un panel Credits
        Debug.Log("Cr�dits ouverts");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Jeu quitt�");
    }
}
