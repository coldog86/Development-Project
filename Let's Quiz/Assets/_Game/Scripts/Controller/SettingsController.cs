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
        public Toggle soundEffectSwitch;
        public Toggle backgroundMusicSwitch;
        public Toggle notificationSwitch;

        private int _active = 1;
        private int _inactive = 0;
        private string _effectVolumeKey = "SoundEffectVolume";
        private string _effectToggleKey = "SoundEffectToggle";
        private string _musicVolumeKey = "BackgroundMusicVolume";
        private string _musicToggleKey = "BackgroundMusicToggle";
        private string _notificationToggleKey = "BackgroundMusicToggle";

        private PlayerSettings _playerSettings;
        private FeedbackAlert _alert;
        private FeedbackClick _click;
        private FeedbackModal _modal;

        private void Awake()
        {
            LoadPlayerSettings();
            _click = FindObjectOfType<FeedbackClick>();
        }

        public void BackToMenu()
        {
            _click.Play();
            SceneManager.LoadScene(menuIndex, LoadSceneMode.Single);
        }

        public void ToggleSoundEffect()
        {
            if (soundEffectSwitch.isOn)
                SetSoundEffectVolume(unmutedVolume);
            else
                SetSoundEffectVolume(mutedVolume);
            
            SetSoundEffectToggle(soundEffectSwitch.isOn);
        }

        public void ToggleBackgroundMusic()
        {
            if (backgroundMusicSwitch.isOn)
                SetBackgroundMusicVolume(unmutedVolume);
            else
                SetBackgroundMusicVolume(mutedVolume);
            
            SetBackgroundMusicToggle(backgroundMusicSwitch.isOn);
        }

        public void ToggleNotification()
        {
            SetNotificationToggle(notificationSwitch.isOn); 
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
            if (_playerSettings.soundEffectToggled != _active)
                return false;
            else
                return true;
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
            if (_playerSettings.backgroundMusicToggled != _active)
                return false;
            else
                return true;
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
            if (_playerSettings.notificationsToggled != _active)
                return false;
            else
                return true;
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
            {
                _playerSettings.soundEffectToggled = PlayerPrefs.GetInt(_effectToggleKey);

                if (soundEffectSwitch)
                    soundEffectSwitch.isOn = GetSoundEffectToggle();
            }
           

            if (PlayerPrefs.HasKey(_musicToggleKey))
            {
                _playerSettings.backgroundMusicToggled = PlayerPrefs.GetInt(_musicToggleKey);

                if (backgroundMusicSwitch)
                    backgroundMusicSwitch.isOn = GetBackgroundMusicToggle();
            }

            if (PlayerPrefs.HasKey(_notificationToggleKey))
            {
                _playerSettings.notificationsToggled = PlayerPrefs.GetInt(_notificationToggleKey);

                if (notificationSwitch)
                    notificationSwitch.isOn = GetNotificationToggle();
            } 
        }
    }
}