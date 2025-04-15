using System.IO;
using UnityEngine;

namespace Game
{
    public static class SaveManager
    {
        private static string _savePath => Path.Combine(Application.persistentDataPath, "save.json");

        public static void SaveGame(GameData gameData)
        {
            string json = JsonUtility.ToJson(gameData);
            File.WriteAllText(_savePath, json);
        }

        public static GameData LoadGame()
        {
            GameData result = null;
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                result = JsonUtility.FromJson<GameData>(json);
            }
            return result;
        }
    }
}
