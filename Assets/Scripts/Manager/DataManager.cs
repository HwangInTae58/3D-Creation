using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataManager : MonoBehaviour
{
    static GameObject _container;
    static GameObject container { get { return _container; } }

    static DataManager _instance;
    public static DataManager instance {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataManager";
                _instance = _container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    public string GameDataName = "Creation.json";
    public GameData _gameData;
    public GameData gameData { get 
        { 
            if(_gameData == null) 
            { 
                LoadGameData(); 
                SaveGameData(); 
            }
            return _gameData;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataName;
        if (File.Exists(filePath))
        {
            Debug.Log("불러오기");
            string fromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(fromJsonData);
        }
        else
        {
            _gameData = new GameData();
        }
    }
    public void SaveGameData()
    {
        string toJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataName;
        File.WriteAllText(filePath, toJsonData); // 저장된 파일이 있으면 덮어쓰기
    }
}
