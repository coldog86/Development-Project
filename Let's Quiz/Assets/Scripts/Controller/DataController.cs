using UnityEngine;
using Data;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class DataController : MonoBehaviour
    {
        [Header("Component")]
        public RoundData[] AllRoundData;

        [Header("Setting")]
        public int MenuSceneIndex = 1;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(MenuSceneIndex);
        }

        public RoundData GetCurrentRoundData()
        {
            return AllRoundData[0];
        }
    }
}