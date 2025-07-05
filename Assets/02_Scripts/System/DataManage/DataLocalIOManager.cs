using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Scripts used for data I/O
/// file key is the name of data will be saved
/// </summary>
static public class DataLocalIOManager
{
    static private Dictionary<string, string> localDataPaths = new();

    static public void SaveData<T>(T data, string fileKey) where T : class
    {
        if (!localDataPaths.ContainsKey(fileKey)) { initDirectories(fileKey); }
        T copyData = data;
        string crecordJson = JsonConvert.SerializeObject(copyData);
        //byte[] crecordEncrypt = Cryptographer.EncryptStringToBytes_AES(crecordJson);
        //string encryptJson = Convert.ToBase64String(crecordEncrypt);
        //crecordJson = encryptJson;
        File.WriteAllText(localDataPaths[fileKey], crecordJson);
    }
    static public T LoadData<T>(string fileKey, out bool hasfile) where T : class
    {
        if (!localDataPaths.ContainsKey(fileKey)) { initDirectories(fileKey); }
        if (!File.Exists(localDataPaths[fileKey]))
        {
            hasfile = false;
            return default(T);
        }
        else
        {
            hasfile = true;
            string encryptJson = File.ReadAllText(localDataPaths[fileKey]);
            //byte[] crecordEncrypt = Convert.FromBase64String(encryptJson);
            //string crecordJson = Cryptographer.DecryptStringFromBytes_AES(crecordEncrypt);
            //encryptJson = crecordJson;
            return JsonConvert.DeserializeObject<T>(encryptJson);
        }
    }
    static private void initDirectories(string fileKey)
    {
        string datadir = Path.Combine(Application.persistentDataPath, "UnimoData");
        datadir = Path.Combine(datadir, "LocalData");
        //Make folder for savefile
        if (!Directory.Exists(datadir))
            Directory.CreateDirectory(datadir);
        string filename = Path.Combine(datadir, fileKey + ".json");
        localDataPaths.Add(fileKey, filename);
    }
}