using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _optionsMenuPanel;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _questPanel;
    [SerializeField] private GameObject _winPanel;

    [Header("Text Elements")]
    [SerializeField] private TMP_Text _interactionText;

    GameStateManager _gameStateManager;
    QuestManager _questManager;

    private void Awake()
    {
        _gameStateManager = ServiceHub.Instance.GameStateManager;
        _questManager = ServiceHub.Instance.QuestManager;
    }

    private void Start()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager not found in ServiceHub!");
        }
        if (_questManager == null)
        {
            Debug.LogError("QuestManager not found in ServiceHub!");
        }

        HideInteractionPrompt();
        HideQuests();
        HideAllUI();
        ShowMainMenu();
    }

    public void ShowInteractionPrompt()
    {
        if (_interactionText != null)
            _interactionText.gameObject.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        if (_interactionText != null)
            _interactionText.gameObject.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllUI();
        if (_mainMenuPanel != null)
            _mainMenuPanel.SetActive(true);
    }

    public void ShowGameplayUI()
    {
        HideAllUI();
        if (_inGamePanel != null)
            _inGamePanel.SetActive(true);
        ShowQuests();
    }

    public void ShowPauseMenu()
    {
        HideAllUI();
        if (_pauseMenuPanel != null)
            _pauseMenuPanel.SetActive(true);
    }

    public void ShowOptionsMenu()
    {
        HideAllUI();
        if (_optionsMenuPanel != null)
            _optionsMenuPanel.SetActive(true);
    }

    public void ShowWinPanel()
    {
        HideAllUI();
        if (_winPanel != null)
            _winPanel.SetActive(true);
    }

    public void ShowDialoguePanel()
    {
        if (_dialoguePanel != null)
            _dialoguePanel.SetActive(true);
    }

    public void HideDialoguePanel()
    {
        if (_dialoguePanel != null)
            _dialoguePanel.SetActive(false);
    }

    public void ShowQuests()
    {
        if (_questPanel != null)
            _questPanel.SetActive(true);
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        if (_questManager == null) return;

        Transform questListContent = _questManager._questListContent;
        GameObject questEntryPrefab = _questManager._questEntryPrefab;
        GameObject objectiveTextPrefab = _questManager._objectiveTextPrefab;

        if (questListContent == null) return;

        foreach (Transform child in questListContent)
            Destroy(child.gameObject);

        var activeQuests = _questManager.GetActiveQuests();
        if (activeQuests == null) return;

        foreach (var qp in activeQuests)
        {
            if (questEntryPrefab == null || objectiveTextPrefab == null) continue;

            GameObject questEntry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = questEntry.transform.Find("QuestName_TXT")?.GetComponent<TMP_Text>();
            Transform objectiveList = questEntry.transform.Find("Objective_TXT");

            if (questNameText != null)
                questNameText.text = qp.Quest != null ? (qp.Quest._questTitle ?? qp.Quest.name) : "Unknown Quest";

            if (objectiveList != null)
            {
                int current = qp.CurrentAmount;
                int target = qp.Quest != null ? qp.Quest._targetAmount : 0;
                TMP_Text objText = Instantiate(objectiveTextPrefab, objectiveList).GetComponent<TMP_Text>();
                if (objText != null)
                {
                    if (target > 0)
                        objText.text = $"Progress: {current}/{target}";
                    else
                        objText.text = "";
                }
            }
        }
    }

    public void HideQuests()
    {
        if (_questPanel != null)
            _questPanel.SetActive(false);
    }

    public void HideAllUI()
    {
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
        if (_inGamePanel != null) _inGamePanel.SetActive(false);
        if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(false);
        if (_optionsMenuPanel != null) _optionsMenuPanel.SetActive(false);
        if (_dialoguePanel != null) _dialoguePanel.SetActive(false);
        if (_winPanel != null) _winPanel.SetActive(false);
        HideQuests();
    }

    public void StartGame()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager is null in StartGame!");
            return;
        }
        _gameStateManager.SetState(GameState.Gameplay);
    }

    public void ReturnToMainMenu()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager is null in ReturnToMainMenu!");
            return;
        }
        _gameStateManager.SetState(GameState.MainMenu);
    }

    public void ResumeGame()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager is null in ResumeGame!");
            return;
        }
        Debug.Log("ResumeGame called - transitioning to Gameplay state");
        _gameStateManager.SetState(GameState.Gameplay);
    }

    public void OpenOptions()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager is null in OpenOptions!");
            return;
        }
        _gameStateManager.SetState(GameState.Options);
    }

    public void CloseOptions()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager is null in CloseOptions!");
            return;
        }

        if (_gameStateManager._stateBeforeOptions == GameState.Paused)
        {
            _gameStateManager.SetState(GameState.Paused);
        }
        else if (_gameStateManager._stateBeforeOptions == GameState.Gameplay)
        {
            _gameStateManager.SetState(GameState.MainMenu);
        }
        else
        {
            _gameStateManager.SetState(GameState.MainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        if (_gameStateManager == null)
        {
            Debug.LogError("GameStateManager is null in TogglePause!");
            return;
        }

        // Проверяем, не находимся ли мы в процессе перехода
        if (_gameStateManager._currentState == GameState.Gameplay)
        {
            _gameStateManager.SetState(GameState.Paused);
        }
        else if (_gameStateManager._currentState == GameState.Paused)
        {
            _gameStateManager.SetState(GameState.Gameplay);
        }
    }
}
