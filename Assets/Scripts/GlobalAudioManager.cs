using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using YogiGameCore.Utils;
using YogiGameCore.Utils.MonoExtent;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    public AudioMixer Mixer;

    private AudioSource playing_BGM;
    public string MixerBgmGroupName = "BGM";
    public string MixerEffectGroupName = "EFF";
    public string MixerMasterGroupName = "Master";
    public string MasterVolumePropertyName = "MASTER_Volume";
    public string BgmVolumePropertyName = "BGM_Volume";
    public string EffectVolumePropertyName = "EFF_Volume";

    public Vector2 MinMaxSoundValue = new Vector2(-80, 20);
    public bool isPlayBGMOnStart = true;
    public AudioClip bgmClip;

    AudioMixerGroup bgmGroup, effGroup;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        bgmGroup = Mixer.FindMatchingGroups(MixerBgmGroupName)[0];
        effGroup = Mixer.FindMatchingGroups(MixerEffectGroupName)[0];
    }

    private void Start()
    {
        playing_BGM = this.gameObject.GetOrAddComponent<AudioSource>();
        if (isPlayBGMOnStart)
            PlayBGM(bgmClip);
    }
    public void RestartBGM()
    {
        if (playing_BGM != null)
        {
            playing_BGM.time = 0;
        }
    }
    public void PlayBGM(string audioPath)
    {
        var audio = Resources.Load<AudioClip>(audioPath);
        PlayBGM(audio);
    }
    public void PlayBGM(AudioClip clip)
    {
        playing_BGM.clip = clip;
        if (playing_BGM != null) playing_BGM.Stop();
        playing_BGM.outputAudioMixerGroup = bgmGroup;
        playing_BGM.Play();
    }

    [Button]
    public AudioSource PlayEff(string audioPath, float spatialBlendValue = 1.0f)
    {
        GameObject go = new GameObject();
        var audioSource = go.AddComponent<AudioSource>();
        var clip = Resources.Load<AudioClip>(audioPath);
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = effGroup;
        audioSource.Play();
        audioSource.spatialBlend = spatialBlendValue;
        go.DestroySelfAfterDelayGracefully(clip.length);
        return audioSource;
    }
    public float GetMASTER_Volume()
    {
        Mixer.GetFloat(MasterVolumePropertyName, out float value);
        value = RemapFloat(value);
        return value;
    }

    [Button]
    public void SetMASTER_Volume(float value)
    {
        Mixer.SetFloat(MasterVolumePropertyName, Mathf.Lerp(MinMaxSoundValue.x, MinMaxSoundValue.y, value));
    }
    public float GetBGM_Volume()
    {
        Mixer.GetFloat(BgmVolumePropertyName, out float value);
        value = RemapFloat(value);
        return value;
    }
    [Button]
    public void SetBGM_Volume(float value)
    {
        Mixer.SetFloat(BgmVolumePropertyName, Mathf.Lerp(MinMaxSoundValue.x, MinMaxSoundValue.y, value));
    }
    public float GetEFF_Volume()
    {
        Mixer.GetFloat(EffectVolumePropertyName, out float value);
        value = RemapFloat(value);
        return value;
    }
    [Button]
    public void SetEFF_Volume(float value)
    {
        Mixer.SetFloat(EffectVolumePropertyName, Mathf.Lerp(MinMaxSoundValue.x, MinMaxSoundValue.y, value));
    }
    private float RemapFloat(float value)
    {
        return value * (MinMaxSoundValue.y - MinMaxSoundValue.x) + MinMaxSoundValue.x;
    }
}

