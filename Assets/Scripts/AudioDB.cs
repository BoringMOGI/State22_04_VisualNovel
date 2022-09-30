using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class VoiceGroup
{
    public string fileName;
    public AudioClip[] voices;

    public AudioClip GetVoice(string name)
    {
        return voices.Where(v => v.name.Equals(name)).First();
    }
}

public class AudioDB : MonoBehaviour
{
    public static AudioDB instance;

    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] ses;
    [SerializeField] AudioClip[] uis;

    [SerializeField] VoiceGroup[] voiceGroups;

    Dictionary<string, AudioClip> bgmList;
    Dictionary<string, AudioClip> seList;
    Dictionary<string, AudioClip> uiList;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bgmList = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in bgms)
            bgmList.Add(clip.name, clip);

        seList = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in ses)
            seList.Add(clip.name, clip);

        uiList = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in uis)
            uiList.Add(clip.name, clip);
    }

    public AudioClip GetBGM(string name) { return bgmList[name]; }
    public AudioClip GetSE(string name) { return seList[name]; }
    public AudioClip GetUI(string name) { return uiList[name]; }

    public VoiceGroup GetVoiceGroup(string fileName)
    {
        return voiceGroups.Where(g => g.fileName.Equals(fileName)).First();
    }
}
