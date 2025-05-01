using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UIMinigamePanel : UIBase
{
    private readonly string[] BestScoreArr = { "FlappyBirdBestScore" };
    public static UIMinigamePanel Instance { get; private set; }

    [SerializeField] private Transform minigameSlotRoot;
    [SerializeField] private MiniGameSlot minigameSlotPrefab;
    [SerializeField] private Button minigameStartButton;

    [Header("Leaderboard")]
    [SerializeField] private GameObject leaderboardObj;

    [SerializeField] private TextMeshProUGUI miniGameNameText;
    [SerializeField] private TextMeshProUGUI minigameBestscoreText;
    [SerializeField] private Button exitButton;


    public MiniGameType MinigameType { get; private set; } = MiniGameType.None;

    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    void Start()
    {
        minigameStartButton.onClick.AddListener(() => OnClickStartBtn());

        CreateMiniGameSlot();
        OnClickCloseLeaderboardBtn();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void SelectMiniGame(MiniGameType type)
    {
        MinigameType = type;
        leaderboardObj.SetActive(true);
        miniGameNameText.text = GetSceneName(type);
        minigameBestscoreText.text = $"{PlayerPrefs.GetInt(BestScoreArr[(int)type], 0)} 점";
    }

    string GetSceneName(MiniGameType type)
    {
        return type switch
        {
            MiniGameType.FlappyBird => "FlappyBird",

            _ => string.Empty
        };
    }

    void CreateMiniGameSlot()
    {
        foreach (var data in TableManager.Instance.GetTable<MiniGameTable>().dataDic.Values)
        {
            MiniGameSlot slot = Instantiate(minigameSlotPrefab, minigameSlotRoot);
            slot.SetMiniGameSlot(data);
        }
    }


    void OnClickStartBtn()
    {
        if (MinigameType != MiniGameType.None)
        {
            LoadSceneManager.Instance.LoadSceneAsync(GetSceneName(MinigameType));
        }
    }


    #region ButtonAction

    public void OnClickCloseLeaderboardBtn()
    {
        leaderboardObj.SetActive(false);
    }

    #endregion
}