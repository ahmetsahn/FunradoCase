using System.Collections.Generic;
using System.IO;
using AYellowpaper.SerializedCollections;
using Runtime.Data.Scriptable;
using Runtime.Data.Value;
using UnityEditor;
using UnityEngine;

public class GridPrefabGenerator : EditorWindow
{
    private Transform parentTransform;
    
    private ObjectType selectedEnumType;

    public enum ObjectType
    {
        BlueFrog, GreenFrog, PurpleFrog, RedFrog, YellowFrog,
        BlueCell, GreenCell, PurpleCell, RedCell, YellowCell, EmptyCell,
        BlueGrape, GreenGrape, PurpleGrape, RedGrape, YellowGrape,
        BlueArrow, GreenArrow, PurpleArrow, RedArrow, YellowArrow
    }

    [SerializeField]
    private SerializedDictionary<ObjectType, GameObject> prefabDictionary = new();

    private Dictionary<Vector2Int, float> occupiedCells = new();
    
    private Stack<(Vector2Int cell, GameObject prefab, ObjectType type, float yValue)> undoStack = new();

    [SerializeField]
    private int gridSize = 10;

    private readonly Vector2 cellSize = new Vector2(122, 122);
    private Vector2Int? selectedCellKey = null;

    [SerializeField] private LevelDataListSo levelDataListSo;
    
    [SerializeField]
    private int maxMoves;
    private int _frogCount;

    [MenuItem("Window/Grid Prefab Generator")]
    public static void ShowWindow()
    {
        GetWindow<GridPrefabGenerator>("Grid Prefab Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Prefab Generator", EditorStyles.boldLabel);

        parentTransform = (Transform)EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true);
        selectedEnumType = (ObjectType)EditorGUILayout.EnumPopup("Enum Type", selectedEnumType);
        gridSize = EditorGUILayout.IntField("Grid Size", gridSize);

        GUILayout.Label("Prefab Dictionary", EditorStyles.boldLabel);
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("prefabDictionary");
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

        levelDataListSo = (LevelDataListSo)EditorGUILayout.ObjectField("LevelDataList", levelDataListSo, typeof(LevelDataListSo), true);
        maxMoves = EditorGUILayout.IntField("Max Moves", maxMoves);

        for (int y = 0; y < gridSize; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int cellKey = new Vector2Int(x, y);
                GUI.backgroundColor = selectedCellKey.HasValue && selectedCellKey.Value.Equals(cellKey)
                    ? Color.green
                    : Color.white;

                if (GUILayout.Button($"{x + y * gridSize}", GUILayout.Width(cellSize.x), GUILayout.Height(cellSize.y)))
                {
                    selectedCellKey = cellKey;
                }
            }
            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;

        GUILayout.Space(20);

        bool canAddOrDelete = prefabDictionary.ContainsKey(selectedEnumType);

        if (canAddOrDelete && GUILayout.Button("Add", GUILayout.Height(100)))
        {
            AddPrefab();
        }

        GUILayout.Space(20);

        if (canAddOrDelete && GUILayout.Button("Delete", GUILayout.Height(100)))
        {
            DeletePrefab();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Reset", GUILayout.Height(100)))
        {
            if (EditorUtility.DisplayDialog("Reset Grid", "Are you sure you want to reset the grid? This action cannot be undone.", "Yes", "No"))
            {
                ResetGrid();
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Generate Level", GUILayout.Height(100)))
        {
            GenerateLevel();
        }
    }

    private void AddPrefab()
    {
        if (parentTransform == null)
        {
            Debug.LogWarning("Parent Transform is not set.");
            return;
        }

        if (prefabDictionary.TryGetValue(selectedEnumType, out GameObject prefabToInstantiate))
        {
            if (selectedCellKey.HasValue)
            {
                Vector2Int cellKey = selectedCellKey.Value;

                float previousY = 0;

                if (occupiedCells.ContainsKey(cellKey))
                {
                    previousY = occupiedCells[cellKey];
                }

                float newY = previousY + GetEnumYValue(selectedEnumType);
                Vector3 position = new Vector3(cellKey.x, newY, gridSize - cellKey.y - 1);

                GameObject newPrefab = Instantiate(prefabToInstantiate, position, prefabToInstantiate.transform.rotation, parentTransform);

                undoStack.Push((cellKey, newPrefab, selectedEnumType, newY));
                occupiedCells[cellKey] = newY;
            }
            else
            {
                Debug.LogWarning("No cell selected for adding prefab.");
            }
        }
        else
        {
            Debug.LogWarning("Selected enum type is not found in the dictionary.");
        }
    }

    private void DeletePrefab()
    {
        if (undoStack.Count > 0)
        {
            var lastAction = undoStack.Pop();
            DestroyImmediate(lastAction.prefab);

            Vector2Int cellKey = lastAction.cell;
            ObjectType deletedType = lastAction.type;

            if (occupiedCells.ContainsKey(cellKey))
            {
                float yValueToRemove = GetEnumYValue(deletedType);
                occupiedCells[cellKey] -= yValueToRemove;

                if (occupiedCells[cellKey] <= 0)
                {
                    occupiedCells.Remove(cellKey);
                }
            }
        }
        else
        {
            Debug.LogWarning("No prefab to delete.");
        }
    }

    private void ResetGrid()
    {
        while (undoStack.Count > 0)
        {
            var lastAction = undoStack.Pop();
            DestroyImmediate(lastAction.prefab);
        }
        occupiedCells.Clear();

        Debug.Log("Grid reset completed.");
    }

    private void GenerateLevel()
    {
        if (parentTransform == null)
        {
            Debug.LogWarning("Parent Transform is not set.");
            return;
        }

        // Sayılan frog sayısını parentTransform altında bul ve `frogCount` değerini güncelle
        int countFrogs = 0;
        foreach (Transform child in parentTransform)
        {
            // Çocuğun türü `Frog` olanları say
            if (prefabDictionary.ContainsKey(selectedEnumType) && child.gameObject.name.Contains("Frog"))
            {
                countFrogs++;
            }
        }
        _frogCount = countFrogs;  // `frogCount` değerini güncelle

        // Eğer `levelDataListSo` mevcutsa yeni level verisini oluştur
        if (levelDataListSo != null)
        {
            LevelData newLevelData = new LevelData
            {
                MaxMoves = maxMoves,
                FrogCount = _frogCount
            };

            levelDataListSo.Levels.Add(newLevelData);
            EditorUtility.SetDirty(levelDataListSo);

            Debug.Log("New level data added to LevelDataListSo.");
        }
        else
        {
            Debug.LogError("LevelDataListSo reference is not set.");
        }

        string folderPath = "Assets/Resources/Levels/";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string prefabPath = folderPath + parentTransform.name + ".prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(parentTransform.gameObject, prefabPath);

        if (prefab != null)
        {
            Debug.Log($"Prefab created at: {prefabPath}");
        }
        else
        {
            Debug.LogError("Failed to create prefab.");
        }
    }

    private float GetEnumYValue(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.BlueFrog: case ObjectType.GreenFrog: case ObjectType.PurpleFrog:
            case ObjectType.RedFrog: case ObjectType.YellowFrog: return 0.145f;
            case ObjectType.BlueCell: case ObjectType.GreenCell: case ObjectType.PurpleCell:
            case ObjectType.RedCell: case ObjectType.YellowCell: case ObjectType.EmptyCell: return 0.1f;
            case ObjectType.BlueGrape: case ObjectType.GreenGrape: case ObjectType.PurpleGrape:
            case ObjectType.RedGrape: case ObjectType.YellowGrape: return 0.23f;
            case ObjectType.BlueArrow: case ObjectType.GreenArrow: case ObjectType.PurpleArrow:
            case ObjectType.RedArrow: case ObjectType.YellowArrow: return 0.04f;
            default: return 0;
        }
    }
}
