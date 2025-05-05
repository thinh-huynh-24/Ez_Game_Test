using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIAnimation.Addon
{
    public class LoadNextLevel : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Demo");
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}