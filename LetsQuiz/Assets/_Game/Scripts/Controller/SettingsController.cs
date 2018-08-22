using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class SettingsController : Singleton<SettingsController>
    {
        #region variables

        [Header("Components")]
        public AudioMixer MasterMixer;

        private Toggle _soundEffectSwitch;
        private Toggle _backgroundMusicSwitch;
        private Toggle _notificationSwitch;

        #endregion variables

        #region properties

        public PlayerSettings Settings;

        #endregion properties

        #region methods

        #region unity

        private void Start()
        {
            Load();
        }

        #endregion unity

        #region navigation specific

        public void BackToMenu()
        {
            FeedbackClick.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion navigation specific

        #region user interaction

        // set the sound effect mixer value based on toggle status
        public void ToggleSoundEffect(bool status)
        {
            FeedbackClick.Play();

            Settings.SoundEffectToggle = status ? 1 : 0;

            if (status)
                MasterMixer.SetFloat(DataHelper.AudioParameter.SOUND_EFFECT, DataHelper.AudioParameter.UNMUTED_VOLUME);
            else
                MasterMixer.SetFloat(DataHelper.AudioParameter.SOUND_EFFECT, DataHelper.AudioParameter.MUTED_VOLUME);

            MasterMixer.GetFloat(DataHelper.AudioParameter.SOUND_EFFECT, out Settings.SoundEffectVolume);
            PlayerPrefs.SetInt(DataHelper.PlayerSettingsKey.EFFECT_TOGGLE, Settings.SoundEffectToggle);
            PlayerPrefs.SetFloat(DataHelper.PlayerSettingsKey.EFFECT_VOLUME, Settings.SoundEffectVolume);

            Save();
        }

        // set the background music mixer value based on toggle status
        public void ToggleBackgroundMusic(bool status)
        {
            FeedbackClick.Play();

            Settings.BackgroundMusicToggle = status ? 1 : 0;

            if (status)
                MasterMixer.SetFloat(DataHelper.AudioParameter.BACKGROUND_MUSIC, DataHelper.AudioParameter.UNMUTED_VOLUME);
            else
                MasterMixer.SetFloat(DataHelper.AudioParameter.BACKGROUND_MUSIC, DataHelper.AudioParameter.MUTED_VOLUME);

            MasterMixer.GetFloat(DataHelper.AudioParameter.BACKGROUND_MUSIC, out Settings.BackgroundMusicVolume);
            PlayerPrefs.SetInt(DataHelper.PlayerSettingsKey.MUSIC_TOGGLE, Settings.BackgroundMusicToggle);
            PlayerPrefs.SetFloat(DataHelper.PlayerSettingsKey.MUSIC_VOLUME, Settings.BackgroundMusicVolume);

            Save();
        }

        // set the push notification based on toggle status
        public void ToggleNotification(bool status)
        {
            FeedbackClick.Play();

            Settings.NotificationsToggle = status ? 1 : 0;

            if (FirebaseController.Initialised)
                FirebaseController.Instance.ToogleSubscription(status);

            PlayerPrefs.SetInt(DataHelper.PlayerSettingsKey.NOTIFICATION_TOGGLE, Settings.NotificationsToggle);

            Save();
        }

        #endregion user interaction

        #region load settings

        // save player settings to playerprefs
        public void Save()
        {
            PlayerPrefs.Save();
        }

        // load player settings from playerprefs
        public void Load()
        {
            if (PlayerPrefs.HasKey(DataHelper.PlayerDataKey.ID))
            {
                Settings = new PlayerSettings
                {
                    SoundEffectVolume = PlayerPrefs.GetFloat(DataHelper.PlayerSettingsKey.EFFECT_VOLUME),
                    SoundEffectToggle = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.EFFECT_TOGGLE),
                    BackgroundMusicVolume = PlayerPrefs.GetFloat(DataHelper.PlayerSettingsKey.MUSIC_VOLUME),
                    BackgroundMusicToggle = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.MUSIC_TOGGLE),
                    NotificationsToggle = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.NOTIFICATION_TOGGLE)
                };
            }
            else
                Settings = new PlayerSettings();

            if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Settings)
            {
                _soundEffectSwitch = GameObject.Find("SoundEffectToggle").GetComponent<Toggle>();
                _soundEffectSwitch.isOn = Settings.SoundEffectToggle == 1 ? true : false;

                _backgroundMusicSwitch = GameObject.Find("BackgroundMusicToggle").GetComponent<Toggle>();
                _backgroundMusicSwitch.isOn = Settings.BackgroundMusicToggle == 1 ? true : false;

                _notificationSwitch = GameObject.Find("NotificationToggle").GetComponent<Toggle>();
                _notificationSwitch.isOn = Settings.NotificationsToggle == 1 ? true : false;
            }

            MasterMixer.SetFloat(DataHelper.AudioParameter.SOUND_EFFECT, Settings.SoundEffectVolume);
            MasterMixer.SetFloat(DataHelper.AudioParameter.BACKGROUND_MUSIC, Settings.BackgroundMusicVolume);
        }

        #endregion load settings

        #endregion methods
    }
}