
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using static Define;
[System.Serializable]
public class UserData
{
    public void SetDefaultData()
    {
    }
}

//파일에 저장할, 영구적인 저장이 필요한 데이터를 관리.
//ex) 재화. 보유아이템.
public class UserDataManager : IManager
{
    private UserData _userData = new UserData();
    
    
    private string _fullPath => Path.Combine(Application.persistentDataPath, "save.json");
    void IManager.Init()
    {
        if (LoadData()) return;
            
        _userData.SetDefaultData();
        SaveData();
    }

    void IManager.Clear()
    {
    }

    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(_userData);
        File.WriteAllText(_fullPath, json);
    }
    public bool LoadData()
    {
        if (!File.Exists(_fullPath))
            return false;

        string json = File.ReadAllText(_fullPath);
        _userData = JsonConvert.DeserializeObject<UserData>(json);
        return true;
    }
   
}