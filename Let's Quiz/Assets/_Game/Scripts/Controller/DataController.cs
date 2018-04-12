using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace _LetsQuiz
{
    public class DataController : MonoBehaviour
    {
        [Header("Server")]
        public string hostURL = "";
        public string connectionFile = "connected.php";

        [Header("Setting")]
        public int loginIndex = 1;
        public float connectionTimeLimit = 10000.0f;

        [Header("Component")]
        private FeedbackAlert _alert;
        private LoadHelper _loadHelper;

        private float _connectionTimer = 0.0f;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(ConnectToServer());

            _alert = FindObjectOfType<FeedbackAlert>();

            if (!_loadHelper)
                _loadHelper = GetComponent<LoadHelper>();
        }

        private IEnumerator RetryConnection()
        {
            _alert.Show("Retrying connection...");
            yield return new WaitForSecondsRealtime(3.0f);
            StartCoroutine(ConnectToServer());
        }

        private IEnumerator ConnectToServer()
        {
            WWW open = new WWW(hostURL + connectionFile);

            while (!open.isDone)
            {
                _connectionTimer += Time.deltaTime;
                if (_connectionTimer > connectionTimeLimit)
                {
                    _alert.Show("Server timeout. Attempting to try again.");
                    StartCoroutine(RetryConnection());
                }
                yield return null;
            }
            if (!open.isDone || open.error != null)
            {
                _alert.Show("Server error. Attempting to try again.");
                StartCoroutine(RetryConnection());
                yield return null;
            }
            else
            {
                yield return open;
                _loadHelper.Load(loginIndex);
                Destroy(_loadHelper);
            }
        }
    }
}