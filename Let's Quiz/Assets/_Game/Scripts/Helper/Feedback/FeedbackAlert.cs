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

        public static FeedbackAlert instance;
        private GameObject _alert;

        private static Text _message;

        private void Awake()
        {
            _alert = Instantiate(prefab);
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_alert);
            _message = _alert.GetComponentInChildren<Text>();
            instance = this;
            instance._alert.SetActive(false);
        }

        public static void Show(string message, float time = 2.5f)
        {
            _message.text = message;
            instance._alert.SetActive(true);

            if (time > 0)
                instance.StartCoroutine(Hide(time));
        }

        public static void Hide()
        {
            instance._alert.SetActive(false);
        }

        private static IEnumerator Hide(float time)
        {
            yield return new WaitForSeconds(time);
            instance._alert.SetActive(false);
        }
    }
}

