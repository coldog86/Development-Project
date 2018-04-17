using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace _LetsQuiz
{

    // NOTE : PLACEHOLDER CODE TO TEST NAVIGATION
    public class DataController : MonoBehaviour
    {
        [Header("Server")]
        public string hostURL = "";
        public string connectionFile = "download.php";

        [Header("Setting")]
        public float connectionTimeLimit = 10000.0f;

        private LoadHelper _loadHelper;

        private float _connectionTimer = 0.0f;

        private void Start()
        {
            _loadHelper = GetComponent<LoadHelper>();
            StartCoroutine(ConnectToServer());
        }

        private void ShowModal()
        {
            FeedbackTwoButtonModal.Show("Error!", "There was a timeout error on attempting to connect to the server. Do you wish to retry?", "Yes", "No", RetryConnection, Quit);
        }

        private void Quit()
        {
            Application.Quit();

            // NOTE : DEBUG PURPOSES ONLY
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

        }

        private void RetryConnection()
        {
            FeedbackAlert.Show("Retrying connection...");
            StartCoroutine(ConnectToServer());
        }

        private IEnumerator ConnectToServer()
        {
            WWW open = new WWW(hostURL + connectionFile);

            while (!open.isDone)
            {
                _connectionTimer += Time.deltaTime;
                if (_connectionTimer > connectionTimeLimit)
                    ShowModal();
                yield return null;
            }
            if (!open.isDone || open.error != null)
            {
                ShowModal();
                yield return null;
            }
            else
            {
                yield return open;
                _loadHelper.Load(BuildIndexHelper.Login);
            }
        }
    }
}