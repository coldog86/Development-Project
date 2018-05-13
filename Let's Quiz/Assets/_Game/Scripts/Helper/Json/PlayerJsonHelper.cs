using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace _LetsQuiz
{
    public static class PlayerJsonHelper
    {
        public static Player LoadPlayerFromServer(string playerString)
        {
            if (!string.IsNullOrEmpty(playerString))
                return JsonUtility.FromJson<Player>(playerString);
            else
                return null;
        }

        public static void SavePlayerToServer()
        {
        }
    }
}

