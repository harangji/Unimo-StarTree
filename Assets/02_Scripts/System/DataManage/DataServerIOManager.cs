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
static public class DataServerIOManager
{
    static private Dictionary<string, string> serverDataPaths = new();

    static public void SaveData<T>(T data, string fileKey) where T : class
    {
        if (!serverDataPaths.ContainsKey(fileKey)) { initDirectories(fileKey); }
        string crecordJson = JsonConvert.SerializeObject(data);
        //byte[] crecordEncrypt = Cryptographer.EncryptStringToBytes_AES(crecordJson);
        //string encryptJson = Convert.ToBase64String(crecordEncrypt);
        //crecordJson = encryptJson;
        File.WriteAllText(serverDataPaths[fileKey], crecordJson);
    }
    static public T LoadData<T>(string fileKey, out bool hasfile) where T : class, new()
    {
        if (!serverDataPaths.ContainsKey(fileKey)) { initDirectories(fileKey); }
        if (!File.Exists(serverDataPaths[fileKey]))
        {
            hasfile = false;
            return default(T);
        }
        else
        {
            hasfile = true;
            string encryptJson = File.ReadAllText(serverDataPaths[fileKey]);
            //byte[] crecordEncrypt = Convert.FromBase64String(encryptJson);
            //string crecordJson = Cryptographer.DecryptStringFromBytes_AES(crecordEncrypt);
            //encryptJson = crecordJson;
            return JsonConvert.DeserializeObject<T>(encryptJson);
        }
    }
    static private void initDirectories(string fileKey)
    {
        string datadir = Path.Combine(Application.persistentDataPath, "UnimoData");
        datadir = Path.Combine(datadir, "ServerData");
        //Make folder for savefile
        if (!Directory.Exists(datadir))
            Directory.CreateDirectory(datadir);
        string filename = Path.Combine(datadir, fileKey + ".json");
        serverDataPaths.Add(fileKey, filename);
    }
}