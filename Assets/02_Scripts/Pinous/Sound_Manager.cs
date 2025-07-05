using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    Bgm,
    Effect,
    Max
}

public class Sound_Manager : MonoBehaviour
{
    public AudioSource[] _audioSources = new AudioSource[(int)Sound.Max];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public static float BGM_volume, FX_volume;

    private GameObject _soundRoot = null;

    public static Sound_Manager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        BGM_volume = PlayerPrefs.GetFloat("BGM");
        FX_volume = PlayerPrefs.GetFloat("FX");
    }

    public void SoundCheck()
    {
        _audioSources[0].volume = BGM_volume;
        _audioSources[1].volume = FX_volume;
    }
    public void ReturnSound(bool SetSoundZero)
    {
        if (SetSoundZero)
        {
            BGM_volume = PlayerPrefs.GetFloat("BGM");
            FX_volume = PlayerPrefs.GetFloat("FX");
        }
        else
        {
            BGM_volume = 0.0f;
            FX_volume = 0.0f;
        }
        SoundCheck();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (_soundRoot == null)
        {
            _soundRoot = GameObject.Find("@SoundRoot");
            if (_soundRoot == null)
            {
                _soundRoot = new GameObject { name = "@SoundRoot" };
                Object.DontDestroyOnLoad(_soundRoot);

                string[] soundTypeNames = System.Enum.GetNames(typeof(Sound));
                for (int count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    GameObject go = new GameObject { name = soundTypeNames[count] };
                    _audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = _soundRoot.transform;
                }

                _audioSources[(int)Sound.Bgm].loop = true;
            }
        }
      
        SoundCheck();
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
            audioSource.Stop();
        _audioClips.Clear();
    }

    public void SetPitch(Sound type, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)type];
        if (audioSource == null)
            return;

        audioSource.pitch = pitch;
    }
    public void PlayClip(AudioClip clip)
    {
        AudioSource audioSource = _audioSources[(int)Sound.Effect];
        audioSource.PlayOneShot(clip);
    }
    public bool Play(Sound type, string path, float volume = 1.0f, float pitch = 1.0f)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        AudioSource audioSource = _audioSources[(int)type];
        if (path.Contains("Sound/") == false)
        {
            path = string.Format("Sound/{0}", path);
        }

        if (type == Sound.Bgm)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path);

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
        else if (type == Sound.Effect)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
                return false;

            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
            return true;
        }

        return false;
    }

    public void Stop(Sound type)
    {
        AudioSource audioSource = _audioSources[(int)type];
        audioSource.Stop();
    }

    public float GetAudioClipLength(string path)
    {
        AudioClip audioClip = GetAudioClip(path);
        if (audioClip == null)
            return 0.0f;
        return audioClip.length;
    }

    private AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;

        if (_audioClips.TryGetValue(path, out audioClip))
            return audioClip;

        audioClip = Resources.Load<AudioClip>(path);

        _audioClips.Add(path, audioClip);
        return audioClip;
    }
}
