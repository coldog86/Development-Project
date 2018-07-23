using UnityEngine;

namespace _LetsQuiz
{
    public class FeedbackClick : MonoBehaviour
    {
        #region variables

        public AudioSource source;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region audio specific

        public void Play()
        {
            source.Play();
        }

        #endregion

        #endregion
    }
}