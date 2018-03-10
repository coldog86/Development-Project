using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class MenuController : MonoBehaviour
    {
        [Header("Setting")]
        public int GameSceneIndex = 2;

        private void StartGame()
        {
            SceneManager.LoadScene(GameSceneIndex);
        }
    }
}