using UnityEngine;

namespace _LetsQuiz
{
    public class PlayerSettings
    {
        [Header("Sound Effects")]
        public int soundEffectVolume = -10;
        public int soundEffectToggled = 1;

        [Header("Background Music")]
        public int backgroundMusicVolume = -10;
        public int backgroundMusicToggled = 1;

        [Header("Notifications")]
        public int notificationsToggled = 1;
    }
}