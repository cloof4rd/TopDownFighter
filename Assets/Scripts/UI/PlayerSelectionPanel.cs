using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YogiGameCore.Utils.MonoExtent;

public class PlayerSelectionPanel : MonoBehaviour
{
    public Button prevBtn, nextBtn;
    public Button readyBtn;
    public Role role;
    public static List<PlayerSelectionPanel> allPanel = new List<PlayerSelectionPanel>();
    public int currentIndex
    {
        get
        {
            return _currentIndex;
        }
        set
        {
            _currentIndex = value;
            while (_currentIndex > ConstConfig.RoleMinMaxIndex.y)
                _currentIndex -= ConstConfig.RoleMinMaxIndex.y;
            while (_currentIndex < ConstConfig.RoleMinMaxIndex.x)
                _currentIndex += ConstConfig.RoleMinMaxIndex.y;
            UpdateByIndex();
        }
    }
    private int _currentIndex;

    public int playerIndex;
    private bool isReady;

    private void OnEnable()
    {
        allPanel.Add(this);
    }
    private void OnDisable()
    {
        allPanel.Remove(this);
    }
    private void Update()
    {
        if (allPanel.All(panel => panel.isReady))
            SceneManager.LoadScene(ConstConfig.BATTLE_SCENE_INDEX);
    }

    public void Awake()
    {
        readyBtn.onClick.AddListener(OnReady);
        prevBtn.onClick.AddListener(OnPrev);
        nextBtn.onClick.AddListener(OnNext);

        currentIndex = PlayerPrefs.GetInt($"selectedCharacterP{playerIndex}", 1);
    }
    [Button]
    public void Test(int index)
    {
        PlayerPrefs.SetInt($"selectedCharacterP{1}", index);
        PlayerPrefs.SetInt($"selectedCharacterP{2}", index);

    }

    private void OnNext()
    {
        currentIndex++;
    }

    private void OnPrev()
    {
        currentIndex--;
    }

    void UpdateByIndex()
    {
        role.Init(currentIndex);
    }

    void OnReady()
    {
        readyBtn.gameObject.SetActive(false);
        prevBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        PlayerPrefs.SetInt($"selectedCharacterP{playerIndex}", currentIndex);
        isReady = true;
    }
}
