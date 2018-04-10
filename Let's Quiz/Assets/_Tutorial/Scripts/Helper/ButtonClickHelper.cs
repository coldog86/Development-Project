using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Tutorial
{
    public class ButtonClickHelper : MonoBehaviour
    {

        [Header("Component")]
        public AudioSource audioSource;

        void Start()
        {
            if (!audioSource)
                audioSource = FindObjectOfType<AudioSource>();
        }

        public void HandleClick()
        {
            audioSource.Play();
        }
    }
}