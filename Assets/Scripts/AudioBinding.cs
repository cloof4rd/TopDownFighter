using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioBinding : MonoBehaviour
{
    public static AudioBinding Instance;

    public string HitSoundAudioPath, HitBlockedAudioPath;
    public string SwingAudioPath;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var sceneID = SceneManager.GetActiveScene().buildIndex;
        if (sceneID == ConstConfig.BATTLE_SCENE_INDEX)
        {
            GlobalAudioManager.Instance.SetBGM_Volume(ConstConfig.BattleBGMSound);
        }
        SceneManager.activeSceneChanged += (scene, newScene) =>
        {
            if (newScene.buildIndex == ConstConfig.BATTLE_SCENE_INDEX)
            {
                GlobalAudioManager.Instance.SetBGM_Volume(ConstConfig.BattleBGMSound);
            }
            if (scene.buildIndex == ConstConfig.BATTLE_SCENE_INDEX)
            {
                GlobalAudioManager.Instance.SetBGM_Volume(ConstConfig.CommonSound);
            }
        };

        Role.OnPlayerBeHit += (role) =>
        {
            var audioSource = GlobalAudioManager.Instance.PlayEff(HitSoundAudioPath);
            audioSource.transform.position = role.transform.position;
        };
        Role.OnPlayerBlocked += (role) =>
        {
            var audioSource = GlobalAudioManager.Instance.PlayEff(HitBlockedAudioPath);
            audioSource.transform.position = role.transform.position;
        };
    }

    public void PlaySwingSound(Vector3 position)
    {
        var audioSource = GlobalAudioManager.Instance.PlayEff(SwingAudioPath);
        audioSource.transform.position = position;
    }
}
