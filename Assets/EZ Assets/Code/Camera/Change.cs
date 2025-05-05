using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    public void LoadGameScene()
    {
        Debug.Log("Load Game");
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        Debug.Log("Load Main Menu");
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
