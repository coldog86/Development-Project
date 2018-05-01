using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _LetsQuiz
{
    public class FeedbackMusic : MonoBehaviour
    {
        public AudioSource source;
        public AudioClip backgroundMusicClip;
        public AudioClip gameMusicClip;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void switchClip()
        {
            
        }

        public void Play()
        {
        }

        public void Stop()
        {
        }
    }
}