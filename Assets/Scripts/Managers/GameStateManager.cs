using UnityEngine;

public enum GameState
{
    None,
    Init,
    MainMenu,
    Gameplay,
    Paused,
    Options,
    Win
}

public class GameStateManager : MonoBehaviour
{
    ServiceHub _serviceHub;
    UIManager _uiManager;

    public GameState _currentState { get; private set; }
    public GameState _previousState { get; private set; }
    public GameState _stateBeforeOptions { get; private set; }

    [SerializeField] string _currentStateName;
    [SerializeField] string _previousStateName;

    private bool _isTransitioning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _serviceHub = ServiceHub.Instance;
        _uiManager = _serviceHub.UIManager;

        SetState(GameState.Init);
    }

    public void SetState(GameState newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        if (_isTransitioning)
        {
            Debug.LogWarning($"Attempted to change state to {newState} while already transitioning. Ignoring.");
            return;
        }

        if (newState == GameState.Options)
        {
            _stateBeforeOptions = _currentState;
        }
        
        _previousState = _currentState;
        _currentState = newState;

        Debug.Log($"GameState changed from {_previousState} to {_currentState}");

        _previousStateName = _previousState.ToString();
        _currentStateName = _currentState.ToString();

        _isTransitioning = true;
        OnStateChange(_previousState, _currentState);
        _isTransitioning = false;
    }

    private void OnStateChange(GameState previousState, GameState currentState)
    {
        switch (currentState)
        {
            case GameState.Init:
                Time.timeScale = 1f;
                break;
            case GameState.MainMenu:
                Time.timeScale = 1f;
                _uiManager.ShowMainMenu();
                break;
            case GameState.Gameplay:
                Time.timeScale = 1f;
                _uiManager.ShowGameplayUI();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                _uiManager.ShowPauseMenu();
                break;
            case GameState.Options:
                Time.timeScale = 0f;
                _uiManager.ShowOptionsMenu();
                break;
            case GameState.Win:
                Time.timeScale = 0f;
                _uiManager.ShowWinPanel();
                break;
        }
    }
}
