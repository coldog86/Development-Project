using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _LetsQuiz
{
    public class FeedbackMusic : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}