using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _LetsQuiz
{
    #if PLATFORM_ANDROID
    public class AndroidBackHelper : MonoBehaviour
    {
        public void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Splash || SceneManager.GetActiveScene().buildIndex == BuildIndex.Login || SceneManager.GetActiveScene().buildIndex == BuildIndex.Menu)
            {
                if (Input.GetKey(KeyCode.Escape))
                    FeedbackTwoButtonModal.Show("Are you sure?", "Are you sure you want to quit?", "Yes", "No", Application.Quit, FeedbackTwoButtonModal.Hide);
                
            }
            else
            {
                if (Input.GetKey(KeyCode.Escape))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
            }
                
        }
    }
    #endif
}

