using UnityEngine;

[System.Serializable]
public class Profile
{
    public string name;
    public int age;

    public Profile(string name, int age)
    {
        this.name = name;
        this.age = age;
    }

    public Profile() { }
}


public class TestEasySave : MonoBehaviour
{
    [SerializeField] Profile[] loadProfiles;
    
    void Start()
    {
        Profile[] profiles = new Profile[2]
        {
            new Profile("aa", 20),
            new Profile("bb", 30)
        };
        System.DateTime nowTime = System.DateTime.Now;


            ES3File es3File = new ES3File("SaveFile_Profile.es3");
            es3File.Save("profiles", profiles);
            es3File.Save("nowTime", nowTime);
            es3File.Sync();
            
            // ES3File es3File = new ES3File("SaveFile_Profile.es3");
            if (TryLoad("profiles", es3File, out Profile[] _profiles)) loadProfiles = _profiles;
            if (TryLoad("nowTime", es3File, out System.DateTime _nowTime)) nowTime = _nowTime;

            print(nowTime);
            Debug.Log(loadProfiles[0].name);
    }

    bool TryLoad<T>(string key, ES3File file, out T t) 
    {
        if (file.KeyExists(key)) 
        {
            t = file.Load<T>(key);
            return true;
        }
        t = default;
        return false;
    }
}