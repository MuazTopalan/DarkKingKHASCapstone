using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void Restart()
    {
        FindObjectOfType<AudioManager>().Stop("MenuOST");
        FindObjectOfType<AudioManager>().Play("LetterSound");
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("OST");

    }

    public void GoBackToMenu()
    {
        FindObjectOfType<AudioManager>().Play("LetterSound");
        SceneManager.LoadScene(0);
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