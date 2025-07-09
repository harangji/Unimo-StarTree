using System;
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
    
    private void Start()
    {
        Profile[] profiles = new Profile[2]
        {
            new Profile("���϶�", 20),
            new Profile("�϶���", 30)
        };
        
        EasySaveManager.Instance.SaveBuffered("profiles", profiles);
        EasySaveManager.Instance.CommitBuffered();

        if (EasySaveManager.Instance.TryLoad("profiles", out Profile[] _profiles)) //Ű Ž�� �� out���� ��ȯ
        {
            loadProfiles = _profiles;
        }
        else
        {
            Debug.LogWarning("Failed to load profiles");
        }
    }
}