using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Configuration;

namespace _LetsQuiz
{
    // NOTE : PLACEHOLDER CODE TO TEST NAVIGATION
    public class DataController : MonoBehaviour
    {
        #region variables

        [Header("Server")]
        public string hostURL = "";
        public string connectionFile = "";

        [Header("Setting")]
        public float connectionTimeLimit = 10000.0f;

        private SettingsController _settingsController;
        private LoadHelper _loadHelper;
        private float _connectionTimer = 0.0f;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            _loadHelper = GetComponent<LoadHelper>();
            _settingsController = GetComponent<SettingsController>();
            _settingsController.LoadPlayerSettings();
            StartCoroutine(ConnectToServer());
            DontDestroyOnLoad(gameObject);
        }

        private void Quit()
        {
            Application.Quit();

            // NOTE : DEBUG PURPOSES ONLY
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

        }

        #endregion

        #region server specific

        private void RetryConnection()
        {
            FeedbackAlert.Show("Retrying connection...", 1.0f);
            StartCoroutine(ConnectToServer());
        }

        private IEnumerator ConnectToServer()
        {
            WWW open = new WWW(hostURL + connectionFile);

            while (!open.isDone)
            {
                _connectionTimer += Time.deltaTime;
                if (_connectionTimer > connectionTimeLimit)
                    ShowModal("There was a timeout error connecting to the server.");
                yield return null;
            }
            if (!open.isDone || open.error != null)
            {
                ShowModal("There was an error connecting to the server.");
                yield return null;
            }
            else
            {
                yield return open;

                var playerType = _settingsController.GetPlayerType();

                if (playerType == PlayerStatus.New)
                    _loadHelper.Load(BuildIndex.Login);
                else if (playerType == PlayerStatus.Existing || playerType == PlayerStatus.Guest)
                    _loadHelper.Load(BuildIndex.Menu);
            }
        }

        #endregion

        #region feedback specific

        private void ShowModal(string message)
        {
            FeedbackTwoButtonModal.Show("Error!", message + "\nDo you wish to retry?", "Yes", "No", RetryConnection, Quit);
        }

        #endregion

        #endregion
    }
}