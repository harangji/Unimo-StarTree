using UnityEngine;

// public static class SaveKeys
// {
//     public const string SaveFile = "UserData.es3";
//     
//     // public const string Coins = "Coins";
//     // public const string Settings = "Settings";
// }
    
public static class EasySaveManager
{
    //private static readonly ES3Settings settings = new ES3Settings(SaveKeys.SaveFile); //easySave �ν��Ͻ� ����
    private static readonly ES3Settings UserDataSettings = new ES3Settings("UserData.es3"); //ES3Settings �ν��Ͻ� ����
    private static readonly ES3File UserDataES3File = new ES3File(UserDataSettings);
    
    // ES3Settings ��� ����
    public static void Save<T>(string key, T value)
    {
        ES3.Save<T>(key, value, UserDataSettings);
    }

    // ES3File ���ۿ� ���� ���� - CommitBuffered() �� �ѹ���
    public static void SaveBuffered<T>(string key, T value)
    {
        UserDataES3File.Save<T>(key, value);
    }

    // ES3File ���ۿ� ���� value�� ����
    public static void CommitBuffered()
    {
        UserDataES3File.Sync();
    }
    
    // �ҷ�����
    public static T Load<T>(string key, T defaultValue = default)
    {
        return ES3.KeyExists(key, UserDataSettings.path) 
            ? ES3.Load<T>(key, UserDataSettings)
            : defaultValue;
    }

    // ����
    public static void Delete(string key)
    {
        if (ES3.KeyExists(key, UserDataSettings.path))
            ES3.DeleteKey(key, UserDataSettings);
    }

    // ��ü ����
    public static void DeleteAll()
    {
        ES3.DeleteFile(UserDataSettings.path);
    }
}
