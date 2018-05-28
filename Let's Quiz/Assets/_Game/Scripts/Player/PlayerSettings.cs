using UnityEngine;

namespace _LetsQuiz
{
    public class PlayerSettings
    {
        [Header("Sound Effects")]
        public float soundEffectVolume = -10.0f;
        public int soundEffectToggled = 1;

        [Header("Background Music")]
        public float backgroundMusicVolume = -10.0f;
        public int backgroundMusicToggled = 1;

        [Header("Notifications")]
        public int notificationsToggled = 1;
    }
}