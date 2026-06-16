using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private LevelManager levelManager;

    [Header("Events")]
    [SerializeField] private GameStartedSO gameStartedSO;

    [Header("Panels")]
    [SerializeField] private GameObject preGamePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TransitionBanner transitionBanner;


    private void Awake()
    {
        RegisterToEvents();
    }

    private void RegisterToEvents()
    {
        levelManager.OnLevelStarted += OnLevelStarted;
        levelManager.OnLevelTransition += OnLevelTransition;
        levelManager.OnGameWon += OnGameWon;
        levelManager.OnGameLost += OnGameLost;
    }

    private void OnLevelStarted(int level)
    {
        ShowOnly(inGamePanel);
    }

    private void OnLevelTransition(int level)
    {
        ShowOnly(transitionPanel);

        if (transitionBanner != null)
            transitionBanner.Play(level, levelManager.TransitionDuration);
    }

    private void OnGameWon()
    {
        ShowOnly(winPanel);
    }

    private void OnGameLost()
    {
        ShowOnly(losePanel);
    }

    private void ShowOnly(GameObject panel)
    {
        SetPanel(preGamePanel, panel);
        SetPanel(winPanel, panel);
        SetPanel(losePanel, panel);
        SetPanel(inGamePanel, panel);
        SetPanel(transitionPanel, panel);
    }

    private void SetPanel(GameObject panel, GameObject active)
    {
        if (panel != null)
            panel.SetActive(panel == active);
    }

    private void Start()
    {
        ShowOnly(preGamePanel);
    }

    public void StartGame()
    {
        gameStartedSO.Fire();
    }

    public void Retry()
    {
        levelManager.RestartLevel();
    }

    private void OnDestroy()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        levelManager.OnLevelStarted -= OnLevelStarted;
        levelManager.OnLevelTransition -= OnLevelTransition;
        levelManager.OnGameWon -= OnGameWon;
        levelManager.OnGameLost -= OnGameLost;
    }
}