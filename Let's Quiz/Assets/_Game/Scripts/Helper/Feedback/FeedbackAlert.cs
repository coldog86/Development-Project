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

        private Text _message;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Show(string message, float time = 2.5f)
        {
            var alert = (GameObject)Instantiate(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            _message = alert.GetComponentInChildren<Text>();
            _message.text = message;
            alert.SetActive(true);
            StartCoroutine(Hide(alert, time));
        }

        private IEnumerator Hide(GameObject alert, float time)
        {
            yield return new WaitForSeconds(time);
            alert.SetActive(false);
            if (!alert.activeSelf)
                Destroy(alert);
        }

        public void Hide(GameObject alert)
        {
            alert.SetActive(false);
            if (!alert.activeSelf)
                Destroy(alert);
        }
    }
}

