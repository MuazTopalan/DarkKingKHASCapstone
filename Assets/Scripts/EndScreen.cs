using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void GoBackToMenu()
    {
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