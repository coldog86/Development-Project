using UnityEngine;
using Data;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class DataController : MonoBehaviour
    {
        [Header("Component")]
        public RoundData[] allRoundData;

        [Header("Setting")]
        public int menuSceneIndex = 1;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(menuSceneIndex, LoadSceneMode.Single);
        }

        public RoundData GetCurrentRoundData()
        {
            return allRoundData[0];
        }
    }
}