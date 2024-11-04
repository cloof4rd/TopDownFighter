using UnityEngine;

public static class Main
{
    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        var audioManager = Resources.Load<GlobalAudioManager>("GlobalAudioManager");
        GameObject.Instantiate(audioManager);
    }
}
