using System.IO;
using Runtime.Data.Value;
using UnityEngine;

namespace Runtime.Managers
{
    public class SaveManager
    {
        private const string SAVE_FILE_NAME = "playerData.json";

        private string GetSaveFilePath()
        {
            return Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        }
        
        public void SaveLevelIndex(int level)
        {
            PlayerData data = new PlayerData
            {
                Level = level
            };

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetSaveFilePath(), json);
        }
        
        public int LoadLevelIndex()
        {
            string filePath = GetSaveFilePath();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                
                return data.Level;
            }
            
            return 0;
        }
    }
}