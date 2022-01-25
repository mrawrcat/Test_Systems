using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataSystem
{

    public static void SaveGameData(GameManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GameData.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        StoreData data = new StoreData(manager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StoreData loadData()
    {
        string path = Application.persistentDataPath + "/GameData.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            StoreData data = formatter.Deserialize(stream) as StoreData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("SaveData Not Found in " + path);
            return null;
        }
    }
}