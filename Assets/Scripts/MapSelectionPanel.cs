using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectionPanel : MonoBehaviour
{
    public Image mapImg;
    public int mapIndex;
    public Vector2Int mapMinMaxIndex = new Vector2Int(1, 10);
    public Button preBtn, nextBtn, confirmBtn;

    private void Awake()
    {
        preBtn.onClick.AddListener(Previous);
        nextBtn.onClick.AddListener(Next);
        confirmBtn.onClick.AddListener(OnConfirm);
    }

    private void OnEnable()
    {
        mapIndex = PlayerPrefs.GetInt(ConstConfig.MAP_SELECT_KEY, mapMinMaxIndex.x);
        UpdateMap(mapIndex);
        EventSystem.current.SetSelectedGameObject(preBtn.gameObject);
    }
    public void Next()
    {
        mapIndex++;
        if (mapIndex > mapMinMaxIndex.y)
        {
            mapIndex = mapMinMaxIndex.x;
        }
        UpdateMap(mapIndex);
    }
    public void Previous()
    {
        mapIndex--;
        if (mapIndex < mapMinMaxIndex.x)
        {
            mapIndex = mapMinMaxIndex.y;
        }
        UpdateMap(mapIndex);
    }
    private void OnConfirm()
    {
        SceneManager.LoadScene(ConstConfig.BATTLE_SCENE_INDEX);
    }

    public void UpdateMap(int index)
    {
        PlayerPrefs.SetInt(ConstConfig.MAP_SELECT_KEY, index);
        this.mapImg.sprite = Resources.Load<Sprite>($"{ConstConfig.MAP_RESOURCE_PATH}{index}");
    }
}
