using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackClick : MonoBehaviour
{
    public AudioSource source;
    public Button[] buttons;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }

    public void Play()
    {
        source.Play();
    }
}
