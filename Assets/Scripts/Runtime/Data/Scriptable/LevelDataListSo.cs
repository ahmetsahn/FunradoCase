using System.Collections.Generic;
using Runtime.Data.Value;
using UnityEngine;

namespace Runtime.Data.Scriptable
{
    [CreateAssetMenu(fileName = "LevelDataList", menuName = "Scriptable Object/LevelDataSo", order = 0)]
    public class LevelDataListSo : ScriptableObject
    {
        public List<LevelData> Levels = new();
    }
}