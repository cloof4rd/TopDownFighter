using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YogiGameCore.Utils;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector2Int minMaxCharacterIndex = new Vector2Int(1, 9);
    public CinemachineTargetGroup targetGroup;
    public CamGroupController camGroupController;
    public GameObject[] maps;

    public Role[] playerRoleArr;

    public bool isDebugPlayer1;
    public int debugPlayer1Index = 1;

    private bool isGamePlaying = true;
    private float gameTime;

    public RoleDetailPanel detailPanel;
    private RoleDescriptionConfig roleDescriptionConfig;

    public float GetGameTime()
    {
        return gameTime;
    }

    private void Awake()
    {
        roleDescriptionConfig = Resources.Load<RoleDescriptionConfig>("RoleDescriptionConfig");
        Instance = this;
        var mapIndex = PlayerPrefs.GetInt(ConstConfig.MAP_SELECT_KEY, 1);
        maps[mapIndex - 1].SetActive(true);
        //this.mapRenderer.sprite = Resources.Load<Sprite>($"{ConstConfig.MAP_RESOURCE_PATH}{mapIndex}");
    }

    private void Start()
    {
        if (isDebugPlayer1)
        {
            playerRoleArr[0].Init(debugPlayer1Index);
        }
        else
        {
            InitCharacterIndexByPlayerIndex(1);
        }
        InitCharacterIndexByPlayerIndex(2);
        InputController.onPause += () => GameManager.Instance.GamePauseToggle();

    }

    private void InitCharacterIndexByPlayerIndex(int playerIndex)
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt($"selectedCharacterP{playerIndex}");
        Role role = playerRoleArr[playerIndex - 1];
        role.Init(selectedCharacterIndex);
        BattleManager.Instance.InitRoleBattleInfo(role, playerIndex);
        role.OnDie += () => GameOver().Forget();
        var data = roleDescriptionConfig.GetDeatilByIndex(selectedCharacterIndex);
        detailPanel.SetDisplay(playerIndex, data.Name);
    }
    public void GamePauseToggle()
    {
        isGamePlaying = !isGamePlaying;
    }
    private async UniTaskVoid GameOver()
    {
        isGamePlaying = false;
        bool isPlayer1Win = BattleManager.Instance.infoArr[0].isWinner;

        await camGroupController
            .OnlyShowSingle(isPlayer1Win ? 0 : 1, 1.0f);

        var controllers = GameObject.FindObjectsOfType<InputController>();
        controllers.ForEach(x =>
        {
            x.isBlockInput = true;
            x.SwitchToUIInput();
        });
        var roles = GameObject.FindObjectsOfType<Role>();
        roles.ForEach(x => x.inputData.Clear());


        BattleManager.Instance.FinishBattle();
    }

    private void FixedUpdate()
    {
        if (!isGamePlaying)
            return;
        gameTime += Time.fixedDeltaTime;
    }
}

