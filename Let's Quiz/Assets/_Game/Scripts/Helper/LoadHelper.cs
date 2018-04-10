using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.Helper
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

