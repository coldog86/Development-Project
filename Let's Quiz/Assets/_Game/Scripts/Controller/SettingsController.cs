using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Runtime.InteropServices;

namespace _LetsQuiz
{
    public class SettingsController : MonoBehaviour
    {
        [Header("Settings")]
        public int menuIndex = 2;
        public float unmutedVolume = -10.0f;
        public float mutedVolume = -80.0f;

        [Header("Componenets")]
        public AudioMixer masterMixer;
        private Toggle _soundEffectSwitch;
        private Toggle _backgroundMusicSwitch;
        private Toggle _notificationSwitch;

        // PlayerPref keys
        private string _effectVolumeKey = "SoundEffectVolume";
        private string _effectToggleKey = "SoundEffectToggle";
        private string _musicVolumeKey = "BackgroundMusicVolume";
        private string _musicToggleKey = "BackgroundMusicToggle";
        private string _notificationToggleKey = "NotificationToggle";

        // int declartions of bools
        private int _active = 1;
        private int _inactive = 0;

        private int _settingsIndex = 6;
        private PlayerSettings _playerSettings;

        private FeedbackAlert _alert;
        private FeedbackClick _click;
        private FeedbackModal _modal;

        private void Awake()
        {
            if (SceneManager.GetActiveScene().buildIndex == _settingsIndex)
            {
                _soundEffectSwitch = GameObject.FindGameObjectWithTag("Toggle_Sound").GetComponent<Toggle>();
                _backgroundMusicSwitch = GameObject.FindGameObjectWithTag("Toggle_Background").GetComponent<Toggle>();
                _notificationSwitch = GameObject.FindGameObjectWithTag("Toggle_Notification").GetComponent<Toggle>();
            }
            _click = FindObjectOfType<FeedbackClick>();
        }

        private void Start()
        {
            LoadPlayerSettings();

            if (_soundEffectSwitch)
                _soundEffectSwitch.isOn = GetSoundEffectToggle();

            if (_backgroundMusicSwitch)
                _backgroundMusicSwitch.isOn = GetBackgroundMusicToggle();

            if (_notificationSwitch)
                _notificationSwitch.isOn = GetNotificationToggle();
        }

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }

        public void ToggleSoundEffect()
        {
            if (_soundEffectSwitch.isOn)
                SetSoundEffectVolume(unmutedVolume);
            else
                SetSoundEffectVolume(mutedVolume);
            
            SetSoundEffectToggle(_soundEffectSwitch.isOn);
        }

        public void ToggleBackgroundMusic()
        {
            if (_backgroundMusicSwitch.isOn)
                SetBackgroundMusicVolume(unmutedVolume);
            else
                SetBackgroundMusicVolume(mutedVolume);
            
            SetBackgroundMusicToggle(_backgroundMusicSwitch.isOn);
        }

        public void ToggleNotification()
        {
            SetNotificationToggle(_notificationSwitch.isOn);
        }

        #region SOUND EFFECT

        #region VOLUME

        public void SetSoundEffectVolume(float volume)
        {
            if (volume != _playerSettings.soundEffectVolume)
            {
                _playerSettings.soundEffectVolume = volume;
                masterMixer.SetFloat("SoundEffectVolume", volume);
                SaveSoundEffectVolume();
            }
        }

        public float GetSoundEffectVolume()
        {
            return _playerSettings.soundEffectVolume;
        }

        private void SaveSoundEffectVolume()
        {
            PlayerPrefs.SetFloat(_effectVolumeKey, _playerSettings.soundEffectVolume);
        }

        #endregion

        #region TOGGLE

        public void SetSoundEffectToggle(bool status)
        {
            var toggleStatus = status ? _active : _inactive;

            if (toggleStatus != _playerSettings.soundEffectToggled)
            {
                _playerSettings.soundEffectToggled = toggleStatus;
                SaveSoundEffectToggle();
            }
        }

        public bool GetSoundEffectToggle()
        {
            var status = _playerSettings.soundEffectToggled == _active ? true : false;
            return status;
        }

        private void SaveSoundEffectToggle()
        {
            PlayerPrefs.SetInt(_effectToggleKey, _playerSettings.soundEffectToggled);
        }

        #endregion

        #endregion

        #region BACKGROUND MUSIC

        #region VOLUME

        public void SetBackgroundMusicVolume(float volume)
        {
            if (volume != _playerSettings.backgroundMusicVolume)
            {
                _playerSettings.backgroundMusicVolume = volume;
                masterMixer.SetFloat("BackgroundMusicVolume", volume);
                SaveBackgroundMusicVolume();
            }
        }

        public float GetBackgroundMusicVolume()
        {
            return _playerSettings.backgroundMusicVolume;
        }

        private void SaveBackgroundMusicVolume()
        {
            PlayerPrefs.SetFloat(_musicVolumeKey, _playerSettings.backgroundMusicVolume);
        }

        #endregion

        #region TOGGLE

        public void SetBackgroundMusicToggle(bool status)
        {
            var toggleStatus = status ? _active : _inactive;

            if (toggleStatus != _playerSettings.backgroundMusicToggled)
            {
                _playerSettings.backgroundMusicToggled = toggleStatus;
                SaveBackgroundMusicToggle();
            }
        }

        public bool GetBackgroundMusicToggle()
        {
            var status = _playerSettings.backgroundMusicToggled == _active ? true : false;
            return status;
        }

        private void SaveBackgroundMusicToggle()
        {
            PlayerPrefs.SetInt(_musicToggleKey, _playerSettings.backgroundMusicToggled);
        }

        #endregion

        #endregion

        #region NOTIFICATIONS

        public void SetNotificationToggle(bool status)
        {
            var toggleStatus = status ? _active : _inactive;

            if (toggleStatus != _playerSettings.notificationsToggled)
            {
                _playerSettings.notificationsToggled = toggleStatus;
                SaveNotificationToggle();
            }
        }

        public bool GetNotificationToggle()
        {
            var status = _playerSettings.notificationsToggled == _active ? true : false;
            return status;
        }

        private void SaveNotificationToggle()
        {
            PlayerPrefs.SetInt(_notificationToggleKey, _playerSettings.notificationsToggled);
        }

        #endregion

        public void LoadPlayerSettings()
        {
            _playerSettings = new PlayerSettings();

            if (PlayerPrefs.HasKey(_effectVolumeKey))
            {
                _playerSettings.soundEffectVolume = PlayerPrefs.GetFloat(_effectVolumeKey);
                masterMixer.SetFloat("SoundEffectVolume", GetSoundEffectVolume());
            }

            if (PlayerPrefs.HasKey(_musicVolumeKey))
            {
                _playerSettings.backgroundMusicVolume = PlayerPrefs.GetFloat(_musicVolumeKey);
                masterMixer.SetFloat("BackgroundMusicVolume", GetBackgroundMusicVolume());
            }

            if (PlayerPrefs.HasKey(_effectToggleKey))
                _playerSettings.soundEffectToggled = PlayerPrefs.GetInt(_effectToggleKey);
           

            if (PlayerPrefs.HasKey(_musicToggleKey))
                _playerSettings.backgroundMusicToggled = PlayerPrefs.GetInt(_musicToggleKey);

            if (PlayerPrefs.HasKey(_notificationToggleKey))
                _playerSettings.notificationsToggled = PlayerPrefs.GetInt(_notificationToggleKey);
        }
    }
}