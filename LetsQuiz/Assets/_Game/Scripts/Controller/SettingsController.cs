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
        private Button _backButton;

        // int declartions of bools
        private const int _toggleActive = 1;
        private const int _toggleInactive = 0;

        #endregion variables

        #region properties

        public PlayerSettings Settings { get; set; }

        #endregion properties

        #region methods

        #region unity

        protected override void OnEnable()
        {
            base.OnEnable();
            DontDestroyOnLoad(gameObject);
            // load the player settings
            Load();
        }

        private void Start()
        {
            // set the toggle status if there are toggles to set

            if (_soundEffectSwitch)
                _soundEffectSwitch.isOn = GetSoundEffectToggle();

            if (_backgroundMusicSwitch)
                _backgroundMusicSwitch.isOn = GetBackgroundMusicToggle();

            if (_notificationSwitch)
                _notificationSwitch.isOn = GetNotificationToggle();
        }

        private void Update()
        {
            if (_soundEffectSwitch != null && _backgroundMusicSwitch != null && _notificationSwitch != null && _backButton != null)
                return;

            if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Settings)
            {
                // get required components
                _soundEffectSwitch = GameObject.FindGameObjectWithTag("Toggle_Sound").GetComponent<Toggle>();
                _backgroundMusicSwitch = GameObject.FindGameObjectWithTag("Toggle_Background").GetComponent<Toggle>();
                _notificationSwitch = GameObject.FindGameObjectWithTag("Toggle_Notification").GetComponent<Toggle>();
                _backButton = GameObject.Find("BackButton").GetComponent<Button>();
                _backButton.onClick.AddListener(BackToMenu);
            }
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
        public void ToggleSoundEffect()
        {
            if (_soundEffectSwitch.isOn)
                SetSoundEffectVolume(DataHelper.AudioParameter.UNMUTED_VOLUME);
            else
                SetSoundEffectVolume(DataHelper.AudioParameter.MUTED_VOLUME);

            SetSoundEffectToggle(_soundEffectSwitch.isOn);
        }

        // set the background music mixer value based on toggle status
        public void ToggleBackgroundMusic()
        {
            if (_backgroundMusicSwitch.isOn)
                SetBackgroundMusicVolume(DataHelper.AudioParameter.UNMUTED_VOLUME);
            else
                SetBackgroundMusicVolume(DataHelper.AudioParameter.MUTED_VOLUME);

            SetBackgroundMusicToggle(_backgroundMusicSwitch.isOn);
        }

        // set the push notification based on toggle status
        public void ToggleNotification()
        {
            SetNotificationToggle(_notificationSwitch.isOn);
        }

        #endregion user interaction

        #region sound effect specific

        #region mixer specific

        // set the sound effect mixer value
        public void SetSoundEffectVolume(int volume)
        {
            if (volume != Settings.SoundEffectVolume)
            {
                Settings.SoundEffectVolume = volume;
                MasterMixer.SetFloat(DataHelper.AudioParameter.SOUND_EFFECT, volume);
                SaveSoundEffectVolume();
            }
        }

        // get the sound effect mixer value
        public float GetSoundEffectVolume()
        {
            return Settings.SoundEffectVolume;
        }

        // save the sound effect mixer value to playerprefs
        private void SaveSoundEffectVolume()
        {
            PlayerPrefs.SetFloat(DataHelper.PlayerSettingsKey.EFFECT_VOLUME, Settings.SoundEffectVolume);
        }

        #endregion mixer specific

        #region toggle specifc

        // set the sound effect toggle value
        public void SetSoundEffectToggle(bool status)
        {
            var toggleStatus = status ? _toggleActive : _toggleInactive;

            if (toggleStatus != Settings.SoundEffectToggle)
            {
                Settings.SoundEffectToggle = toggleStatus;
                SaveSoundEffectToggle();
            }
        }

        // get the sound effect toggle value
        public bool GetSoundEffectToggle()
        {
            var status = Settings.SoundEffectToggle == _toggleActive ? true : false;
            return status;
        }

        // save the sound effect toggle value to playerprefs
        private void SaveSoundEffectToggle()
        {
            PlayerPrefs.SetInt(DataHelper.PlayerSettingsKey.EFFECT_TOGGLE, Settings.SoundEffectToggle);
            PlayerPrefs.Save();
        }

        #endregion toggle specifc

        #endregion sound effect specific

        #region background music specific

        #region mixer specific

        // set the background music mixer value
        public void SetBackgroundMusicVolume(int volume)
        {
            if (volume != Settings.BackgroundMusicVolume)
            {
                Settings.BackgroundMusicVolume = volume;
                MasterMixer.SetFloat(DataHelper.AudioParameter.BACKGROUND_MUSIC, volume);
                SaveBackgroundMusicVolume();
            }
        }

        // get the background music mixer value
        public float GetBackgroundMusicVolume()
        {
            return Settings.BackgroundMusicVolume;
        }

        // save the background music value to playerprefs
        private void SaveBackgroundMusicVolume()
        {
            PlayerPrefs.SetFloat(DataHelper.PlayerSettingsKey.MUSIC_VOLUME, Settings.BackgroundMusicVolume);
        }

        #endregion mixer specific

        #region toggle specific

        // set the background music toggle value
        public void SetBackgroundMusicToggle(bool status)
        {
            var toggleStatus = status ? _toggleActive : _toggleInactive;

            if (toggleStatus != Settings.BackgroundMusicToggle)
            {
                Settings.BackgroundMusicToggle = toggleStatus;
                SaveBackgroundMusicToggle();
            }
        }

        // get the background music toggle value
        public bool GetBackgroundMusicToggle()
        {
            var status = Settings.BackgroundMusicToggle == _toggleActive ? true : false;
            return status;
        }

        // save the background music toggle value to playerprefs
        private void SaveBackgroundMusicToggle()
        {
            PlayerPrefs.SetInt(DataHelper.PlayerSettingsKey.MUSIC_TOGGLE, Settings.BackgroundMusicToggle);
            PlayerPrefs.Save();
        }

        #endregion toggle specific

        #endregion background music specific

        #region notification specific

        // set the notification toggle value
        public void SetNotificationToggle(bool status)
        {
            var toggleStatus = status ? _toggleActive : _toggleInactive;

            if (toggleStatus != Settings.NotificationsToggle)
            {
                FirebaseController.Instance.ToogleSubscription(status);
                Settings.NotificationsToggle = toggleStatus;
                SaveNotificationToggle();
            }
        }

        // get the notification toggle value
        public bool GetNotificationToggle()
        {
            var status = Settings.NotificationsToggle == _toggleActive ? true : false;
            return status;
        }

        // save the notification toggle value to playerprefs
        private void SaveNotificationToggle()
        {
            PlayerPrefs.SetInt(DataHelper.PlayerSettingsKey.NOTIFICATION_TOGGLE, Settings.NotificationsToggle);
            PlayerPrefs.Save();
        }

        #endregion notification specific

        #region load settings

        // load player settings from playerprefs
        public void Load()
        {
            Settings = new PlayerSettings();

            // load sound effect volume
            if (PlayerPrefs.HasKey(DataHelper.PlayerSettingsKey.EFFECT_VOLUME))
            {
                Settings.SoundEffectVolume = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.EFFECT_VOLUME);
                MasterMixer.SetFloat(DataHelper.AudioParameter.SOUND_EFFECT, GetSoundEffectVolume());
            }

            // load background music volume
            if (PlayerPrefs.HasKey(DataHelper.PlayerSettingsKey.MUSIC_VOLUME))
            {
                Settings.BackgroundMusicVolume = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.MUSIC_VOLUME);
                MasterMixer.SetFloat(DataHelper.AudioParameter.BACKGROUND_MUSIC, GetBackgroundMusicVolume());
            }

            // load sound effect toggle status
            if (PlayerPrefs.HasKey(DataHelper.PlayerSettingsKey.EFFECT_TOGGLE))
                Settings.SoundEffectToggle = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.EFFECT_TOGGLE);

            // load background music toggle status
            if (PlayerPrefs.HasKey(DataHelper.PlayerSettingsKey.MUSIC_TOGGLE))
                Settings.BackgroundMusicToggle = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.MUSIC_TOGGLE);

            // load push notifications toggle status
            if (PlayerPrefs.HasKey(DataHelper.PlayerSettingsKey.NOTIFICATION_TOGGLE))
                Settings.NotificationsToggle = PlayerPrefs.GetInt(DataHelper.PlayerSettingsKey.NOTIFICATION_TOGGLE);
        }

        #endregion load settings

        #endregion methods
    }
}