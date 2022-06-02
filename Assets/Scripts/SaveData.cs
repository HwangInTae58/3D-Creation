using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<Friendly> friendPrefab;
    public List<Town> townPrefab;
    public int saveGold;
    public int stage;
    public int maxCount;
    public int bossStage;
    public int collecter;
    public int Oke;
    public int wizard;
    public int Golem;
    public SaveData(int _stage ,int _saveGold ,List<Town> _tList,List<Friendly> _fList, int _maxCount, int _bossStage, int _collecter ,int _Oke, int _wizard, int _Golem)
    {
        stage = _stage;
        saveGold = _saveGold;
        townPrefab = _tList;
        friendPrefab = _fList;
        maxCount = _maxCount;
        bossStage = _bossStage;
        collecter = _collecter;
        Oke = _Oke;
        wizard = _wizard;
        Golem = _Golem;
    }
}
