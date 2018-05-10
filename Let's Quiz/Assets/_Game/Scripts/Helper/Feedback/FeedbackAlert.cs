using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace _LetsQuiz
{
    public class FeedbackAlert : MonoBehaviour
    {
        #region variables

        [Header("Component")]
        public GameObject prefab;
        public static FeedbackAlert instance;
        private GameObject _alert;

        private static Text _message;

        #endregion

        #region methods

        // creates the alert instance
        private void Awake()
        {
            // create instance of alert prefab as gameobject
            _alert = Instantiate(prefab);

            // ensure everything sticks around like a bad smell
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_alert);

            // find all the required components
            _message = _alert.GetComponentInChildren<Text>();

            // set instance
            instance = this;

            // deactivate alert
            instance._alert.SetActive(false);
        }

        // used to show the alert from external sources
        // time is optional
        public static void Show(string message, float time = 2.5f)
        {
            // set the message text 
            _message.text = message;

            // after everything has been set, show the alert
            instance._alert.SetActive(true);

            // start coroutine to hide alert if time is greater than zero
            if (time > 0)
                instance.StartCoroutine(Hide(time));
        }

        // used to hide the alert from external sources if time is set to zero
        public static void Hide()
        {
            // hide the modal
            instance._alert.SetActive(false);
        }

        // used to hide the alert from internal source is time is set to greater than zero
        private static IEnumerator Hide(float time)
        {
            yield return new WaitForSeconds(time);
            instance._alert.SetActive(false);
        }

        #endregion
    }
}

