using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class LoadHelper : MonoBehaviour
    {
        #region variables

        private Slider _slider;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            // get required component
            _slider = FindObjectOfType<Slider>();
        }

        #endregion

        #region load specific

        // used from external sources
        public void Load(int buildIndex)
        {
            // call corotuine
            StartCoroutine(LoadAsync(buildIndex));
        }

        // updates slider values and loads scene
        private IEnumerator LoadAsync(int buildIndex)
        {
            // loads scene via async operation
            AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

            // updates slider value with progress of operation
            while (!operation.isDone)
            {
                _slider.value = operation.progress;
                yield return null;
            }
        }

        #endregion

        #endregion
    }
}