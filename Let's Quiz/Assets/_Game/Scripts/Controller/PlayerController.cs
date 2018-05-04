using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _LetsQuiz
{
    public class PlayerController : MonoBehaviour
    {
        #region variables

        private string _typeKey = "PlayerType";
        private string _usernameKey = "PlayerUsername";
        private string _passwordKey = "PlayerPassword";
        private string _questionDataKey = "PlayerQuestionData";

        private Player _player;

        #endregion

        #region properties

        public string typeKey { get { return _typeKey; } }

        public string usernameKey { get { return _usernameKey; } }

        public string passwordKey { get { return _passwordKey; } }

        public string questionDataKey { get { return _questionDataKey; } }

        #endregion

        #region player status

        // set the player status value
        public void SetPlayerType(int type)
        {
            if (type != _player.type)
            {
                _player.type = type;
                SavePlayerType();
            }
        }

        // get the player status value
        public int GetPlayerType()
        {
            return _player.type;
        }

        // save the player status value in playerprefs
        private void SavePlayerType()
        {
            PlayerPrefs.SetInt(_typeKey, _player.type);
            PlayerPrefs.Save();
        }

        #endregion

        #region player username

        // set the player username value
        public void SetUsername(string username)
        {
            if (username != _player.username)
            {
                _player.username = username;
                SaveUsername();
            }
        }

        // get the player username value
        public string GetUsername()
        {
            return _player.username;
        }

        // save the player username value in playerprefs
        private void SaveUsername()
        {
            PlayerPrefs.SetString(_usernameKey, _player.username);
            PlayerPrefs.Save();
        }

        #endregion

        #region player password

        // set the player password value
        public void SetPassword(string password)
        {
            if (password != _player.password)
            {
                _player.password = password;
                SavePassword();
            }
        }

        // get the player password value
        public string GetPassword()
        {
            return _player.password;
        }

        // save the player password value in playerprefs
        private void SavePassword()
        {
            PlayerPrefs.SetString(_passwordKey, _player.password);
            PlayerPrefs.Save();
        }

        #endregion

        #region player question data

        // set the player question data
        public void SetQuestionData(string questionData)
        {
            if (questionData != _player.questionData)
            {
                _player.questionData = questionData;
                SaveQuestionData();
            }
        }

        // save the player question data in playerprefs
        private void SaveQuestionData()
        {
            PlayerPrefs.SetString(_questionDataKey, _player.questionData);
            PlayerPrefs.Save();
        }

        #endregion

        public void LoadPlayer()
        {
            _player = new Player();

            // load player type
            if (PlayerPrefs.HasKey(_typeKey))
                _player.type = PlayerPrefs.GetInt(_typeKey);

            // load player username
            if (PlayerPrefs.HasKey(_usernameKey))
                _player.username = PlayerPrefs.GetString(_usernameKey);

            // load player username
            if (PlayerPrefs.HasKey(_passwordKey))
                _player.password = PlayerPrefs.GetString(_passwordKey);
        }
    }
}

