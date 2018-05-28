using UnityEngine;

namespace _LetsQuiz
{
    public class FeedbackMusic : MonoBehaviour
    {
        #region variables

        public AudioSource source;
        public AudioClip backgroundMusicClip;
        public AudioClip gameMusicClip;

        #endregion

        #region methods

        #region unity

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region audio specific

        public void PlayBackgroundMusic()
        {
            source.clip = backgroundMusicClip;
            source.Play();
        }

        public void PlayGameMusic()
        {
            source.clip = gameMusicClip;
            source.Play();
        }


        public void Stop()
        {
            source.Stop();
        }

        #endregion

        #endregion
    }
}