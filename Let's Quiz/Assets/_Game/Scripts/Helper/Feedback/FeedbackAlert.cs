using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace _LetsQuiz
{
    public class FeedbackAlert : MonoBehaviour
    {
        [Header("Component")]
        public GameObject prefab;

        private GameObject _alert;
        private Text _message;

        private void Start()
        {
            // keeps object around so that alert can be called whenever
            DontDestroyOnLoad(gameObject);
        }

        public void Show(string message, float time = 2.5f)
        {
            // instantiate the alert
            _alert = (GameObject)Instantiate(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

            // find the required components
            _message = _alert.GetComponentInChildren<Text>();

            // set the message
            _message.text = message;

            // activate the alert
            _alert.SetActive(true);

            // start coroutine to hide the alert
            if (time > 0.0f)
                StartCoroutine(Hide(time));
        }

        private IEnumerator Hide(float time)
        {
            // wait for x amount of time before hiding alert
            yield return new WaitForSeconds(time);

            // deactivate the alert
            _alert.SetActive(false);

            // destroy the prefab
            if (!_alert.activeSelf)
                Destroy(_alert);
        }

        // override method incase alert has a display time of 0
        public void Hide()
        {
            // deactivate the alert
            _alert.SetActive(false);

            // destroy the prefab
            if (!_alert.activeSelf)
                Destroy(_alert);
        }
    }
}

