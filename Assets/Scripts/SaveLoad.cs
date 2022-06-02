using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private static string savePath => Application.persistentDataPath + "/saves/";

    public static void Save(SaveData saveData, string saveFileName)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        string saveJson = JsonUtility.ToJson(saveData);
        string saveFilePath = savePath + saveFileName + ".json";
        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("세이브 성공");
    }
    public static SaveData Load(string saveFileName)
    {
        string saveFilePath = savePath + saveFileName + ".json";
        if (!File.Exists(saveFilePath))
            return null;
        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }
}
