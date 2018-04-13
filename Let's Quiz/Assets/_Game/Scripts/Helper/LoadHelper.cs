using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO : FINISH LOADASYNC

namespace _LetsQuiz
{
    public class LoadHelper : MonoBehaviour
    {
        [Header("Component")]
        public Slider slider;

        public void Load(int buildIndex)
        {
            StartCoroutine(LoadAsync(buildIndex));
        }

        private IEnumerator LoadAsync(int buildIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

            while (!operation.isDone)
            {
                slider.value = operation.progress;
                yield return null;
            }
        }
    }
}