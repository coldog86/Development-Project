using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace _LetsQuiz
{
    public class AudioController : MonoBehaviour
    {
        [Header("Components")]
        public SettingsController _settingsController;

        void Start()
        {
            _settingsController.LoadPlayerSettings();
        }
    }
}