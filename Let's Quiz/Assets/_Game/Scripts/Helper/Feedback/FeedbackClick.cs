using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackClick : MonoBehaviour
{
    public AudioSource source;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Play()
    {
        source.Play();
    }
}
