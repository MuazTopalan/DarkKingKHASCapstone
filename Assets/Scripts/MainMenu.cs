using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("LetterSound");
        FindObjectOfType<AudioManager>().Stop("MenuOST");
        FindObjectOfType<AudioManager>().Play("OST");
    }

    public void QuitButton()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}