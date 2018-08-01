using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    public class AccountController : MonoBehaviour
    {
        #region variables

        private PlayerController _playerController;

        [Header("Components")]
        public Text username;
        public Text email;

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();

            if (!username)
                Debug.Log("Username Input Field is null");
            if (!email)
                Debug.Log("Email Input Field is null");
        }

        private void Start()
        {
            username.text = _playerController.GetUsername();
            email.text = _playerController.GetEmail();
        }

        #endregion

        #region navigation specific

        public void BackToMenu()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion


        #endregion
    }
}