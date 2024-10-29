using UnityEngine;

public class DynamicTextManager : MonoBehaviour
{
    public static DynamicTextManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private DynamicTextData _defaultData;
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private Transform _mainCamera;
    public Transform mainCamera => _mainCamera;

    public static void CreateText2D(Vector2 position, string text, DynamicTextData data = null)
    {
        if (data == null)
            data = instance._defaultData;
        GameObject newText = Instantiate(instance._canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText2D>().Initialise(text, data);
    }

    public static void CreateText(Vector3 position, string text, DynamicTextData data = null)
    {
        if (data == null)
            data = instance._defaultData;
        GameObject newText = Instantiate(instance._canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
    }
}
