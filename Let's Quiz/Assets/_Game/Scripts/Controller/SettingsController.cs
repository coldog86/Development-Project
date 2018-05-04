using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _LetsQuiz
{
    public class SettingsController : MonoBehaviour
    {
        #region variables

        [Header("Settings")]
        [Tooltip("Serialized for debug purposes.")]
        [SerializeField]
        private float unmutedVolume = -10.0f;
        [Tooltip("Serialized for debug purposes.")]
        [SerializeField]
        private float mutedVolume = -80.0f;

        [Header("Componenets")]
        public AudioMixer masterMixer;
        private Toggle _soundEffectSwitch;
        private Toggle _backgroundMusicSwitch;
        private Toggle _notificationSwitch;

        // AudioMixer parameters
        private string _soundEffectParameter = "SoundEffectVolume";
        private string _backgroundMusicParameter = "BackgroundMusicVolume";

        // PlayerPref keys
        private string _effectVolumeKey = "SoundEffectVolume";
        private string _effectToggleKey = "SoundEffectToggle";
        private string _musicVolumeKey = "BackgroundMusicVolume";
        private string _musicToggleKey = "BackgroundMusicToggle";
        private string _notificationToggleKey = "NotificationToggle";


        // int declartions of bools
        private int _toggleActive = 1;
        private int _toggleInactive = 0;

        private PlayerSettings _playerSettings;
        private FeedbackClick _click;

        #endregion

        #region properties

        public string effectVolumeKey { get { return _effectVolumeKey; } }

        public string effectToggleKey { get { return _effectToggleKey; } }

        public string musicVolumeKey { get { return _musicVolumeKey; } }

        public string musicToggleKey { get { return _musicToggleKey; } }

        public string notificationToggleKey { get { return _notificationToggleKey; } }

        #endregion

        #region methods

        #region unity

        private void Awake()
        {
            // only get toggle references if settings scene is active
            if (SceneManager.GetActiveScene().buildIndex == BuildIndex.Settings)
            {
                // get required components
                _soundEffectSwitch = GameObject.FindGameObjectWithTag("Toggle_Sound").GetComponent<Toggle>();
                _backgroundMusicSwitch = GameObject.FindGameObjectWithTag("Toggle_Background").GetComponent<Toggle>();
                _notificationSwitch = GameObject.FindGameObjectWithTag("Toggle_Notification").GetComponent<Toggle>();
            }

            // reference to play annoying clicky sound
            _click = FindObjectOfType<FeedbackClick>();
        }

        private void Start()
        {
            // load the player settings
            LoadPlayerSettings();

            // set the toggle status if there are toggles to set
            if (_soundEffectSwitch)
                _soundEffectSwitch.isOn = GetSoundEffectToggle();

            if (_backgroundMusicSwitch)
                _backgroundMusicSwitch.isOn = GetBackgroundMusicToggle();

            if (_notificationSwitch)
                _notificationSwitch.isOn = GetNotificationToggle();
        }

        #endregion

        #region navigation specific

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(BuildIndex.Menu, LoadSceneMode.Single);
        }

        #endregion

        #region user interaction

        // set the sound effect mixer value based on toggle status
        public void ToggleSoundEffect()
        {
            if (_soundEffectSwitch.isOn)
                SetSoundEffectVolume(unmutedVolume);
            else
                SetSoundEffectVolume(mutedVolume);
            
            SetSoundEffectToggle(_soundEffectSwitch.isOn);
        }

        // set the background music mixer value based on toggle status
        public void ToggleBackgroundMusic()
        {
            if (_backgroundMusicSwitch.isOn)
                SetBackgroundMusicVolume(unmutedVolume);
            else
                SetBackgroundMusicVolume(mutedVolume);
            
            SetBackgroundMusicToggle(_backgroundMusicSwitch.isOn);
        }

        // set the push notification based on toggle status
        public void ToggleNotification()
        {
            SetNotificationToggle(_notificationSwitch.isOn);
        }

        #endregion

        #region sound effect specific

        #region mixer specific

        // set the sound effect mixer value
        public void SetSoundEffectVolume(float volume)
        {
            if (volume != _playerSettings.soundEffectVolume)
            {
                _playerSettings.soundEffectVolume = volume;
                masterMixer.SetFloat("SoundEffectVolume", volume);
                SaveSoundEffectVolume();
            }
        }

        // get the sound effect mixer value
        public float GetSoundEffectVolume()
        {
            return _playerSettings.soundEffectVolume;
        }

        // save the sound effect mixer value to playerprefs
        private void SaveSoundEffectVolume()
        {
            PlayerPrefs.SetFloat(_effectVolumeKey, _playerSettings.soundEffectVolume);
        }

        #endregion

        #region toggle specifc

        // set the sound effect toggle value
        public void SetSoundEffectToggle(bool status)
        {
            var toggleStatus = status ? _toggleActive : _toggleInactive;

            if (toggleStatus != _playerSettings.soundEffectToggled)
            {
                _playerSettings.soundEffectToggled = toggleStatus;
                SaveSoundEffectToggle();
            }
        }

        // get the sound effect toggle value
        public bool GetSoundEffectToggle()
        {
            var status = _playerSettings.soundEffectToggled == _toggleActive ? true : false;
            return status;
        }

        // save the sound effect toggle value to playerprefs
        private void SaveSoundEffectToggle()
        {
            PlayerPrefs.SetInt(_effectToggleKey, _playerSettings.soundEffectToggled);
            PlayerPrefs.Save();
        }

        #endregion

        #endregion

        #region background music specific

        #region mixer specific

        // set the background music mixer value
        public void SetBackgroundMusicVolume(float volume)
        {
            if (volume != _playerSettings.backgroundMusicVolume)
            {
                _playerSettings.backgroundMusicVolume = volume;
                masterMixer.SetFloat("BackgroundMusicVolume", volume);
                SaveBackgroundMusicVolume();
            }
        }

        // get the background music mixer value
        public float GetBackgroundMusicVolume()
        {
            return _playerSettings.backgroundMusicVolume;
        }

        // save the background music value to playerprefs
        private void SaveBackgroundMusicVolume()
        {
            PlayerPrefs.SetFloat(_musicVolumeKey, _playerSettings.backgroundMusicVolume);
        }

        #endregion

        #region toggle specific

        // set the background music toggle value
        public void SetBackgroundMusicToggle(bool status)
        {
            var toggleStatus = status ? _toggleActive : _toggleInactive;

            if (toggleStatus != _playerSettings.backgroundMusicToggled)
            {
                _playerSettings.backgroundMusicToggled = toggleStatus;
                SaveBackgroundMusicToggle();
            }
        }
            
        // get the background music toggle value
        public bool GetBackgroundMusicToggle()
        {
            var status = _playerSettings.backgroundMusicToggled == _toggleActive ? true : false;
            return status;
        }

        // save the background music toggle value to playerprefs
        private void SaveBackgroundMusicToggle()
        {
            PlayerPrefs.SetInt(_musicToggleKey, _playerSettings.backgroundMusicToggled);
            PlayerPrefs.Save();
        }

        #endregion

        #endregion

        #region notification specific

        // set the notification toggle value
        public void SetNotificationToggle(bool status)
        {
            var toggleStatus = status ? _toggleActive : _toggleInactive;

            if (toggleStatus != _playerSettings.notificationsToggled)
            {
                _playerSettings.notificationsToggled = toggleStatus;
                SaveNotificationToggle();
            }
        }

        // get the notification toggle value
        public bool GetNotificationToggle()
        {
            var status = _playerSettings.notificationsToggled == _toggleActive ? true : false;
            return status;
        }

        // save the notification toggle value to playerprefs
        private void SaveNotificationToggle()
        {
            PlayerPrefs.SetInt(_notificationToggleKey, _playerSettings.notificationsToggled);
            PlayerPrefs.Save();
        }

        #endregion

        #region load settings

        // load player settings from playerprefs
        public void LoadPlayerSettings()
        {
            _playerSettings = new PlayerSettings();

            // load sound effect volume
            if (PlayerPrefs.HasKey(_effectVolumeKey))
            {
                _playerSettings.soundEffectVolume = PlayerPrefs.GetFloat(_effectVolumeKey);
                masterMixer.SetFloat(_soundEffectParameter, GetSoundEffectVolume());
            }

            // load background music volume
            if (PlayerPrefs.HasKey(_musicVolumeKey))
            {
                _playerSettings.backgroundMusicVolume = PlayerPrefs.GetFloat(_musicVolumeKey);
                masterMixer.SetFloat(_backgroundMusicParameter, GetBackgroundMusicVolume());
            }

            // load sound effect toggle status
            if (PlayerPrefs.HasKey(_effectToggleKey))
                _playerSettings.soundEffectToggled = PlayerPrefs.GetInt(_effectToggleKey);

            // load background music toggle status
            if (PlayerPrefs.HasKey(_musicToggleKey))
                _playerSettings.backgroundMusicToggled = PlayerPrefs.GetInt(_musicToggleKey);

            // load push notifications toggle status
            if (PlayerPrefs.HasKey(_notificationToggleKey))
                _playerSettings.notificationsToggled = PlayerPrefs.GetInt(_notificationToggleKey);
        }

        #endregion

        #endregion
    }
}