using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneBtn : MonoBehaviour
{
    public int sceneIndex;
    void Start()
    {
        var btn = GetComponent<Button>();
        UnityAction act = () =>
        {
            SceneManager.LoadScene(sceneIndex);
        };
        btn.onClick.AddListener(act);
        PlayerUIController.OnCancelUITrigger += () => act?.Invoke();
        //PlayerUIController
    }

}
