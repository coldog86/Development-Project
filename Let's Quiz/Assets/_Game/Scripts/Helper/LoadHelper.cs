using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// TODO : FINISH LOADASYNC
// NOTE : COMPLETE

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
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                slider.value = progress;
                yield return null;
            }
        }
    }
}