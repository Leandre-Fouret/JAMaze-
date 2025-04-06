using UnityEngine;

public class Scene1 : MonoBehaviour
{
    [SerializeField] Fade fade;

    void Update()
    {
         
        if (Input.anyKeyDown)
        {
            fade.FadeOut(); 
        }
    }
}
