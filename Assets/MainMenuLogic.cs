using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(ConstConfig.ROLE_SELECTION_SCENE_INDEX);
    }
    public void EnterTraining()
    {
        SceneManager.LoadSceneAsync(ConstConfig.TRAINING_SCENE_INDEX);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
