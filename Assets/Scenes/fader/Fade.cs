using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public string LevelToLoad;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}
