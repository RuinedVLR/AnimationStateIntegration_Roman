using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestProgress> _activeQuests;
    public Transform _questListContent;
    public GameObject _questEntryPrefab;
    public GameObject _objectiveTextPrefab;

    public Transform QuestListContent => _questListContent;
    public GameObject QuestEntryPrefab => _questEntryPrefab;
    public GameObject ObjectiveTextPrefab => _objectiveTextPrefab;

    public List<QuestProgress> GetActiveQuests() => _activeQuests;

    private HashSet<int> _completedQuestIds;

    public Quest _startingQuest;

    [Header("Testing")]
    public Quest _testQuest;
    public int _testQuestAmount;
    public List<QuestProgress> _testQuests;

    private UIManager _uiManager;

    void Start()
    {
        _uiManager = ServiceHub.Instance.UIManager;
        _testQuests = new List<QuestProgress>();
        _activeQuests = new List<QuestProgress>();
        _completedQuestIds = new HashSet<int>();

        for (int i = 0; i < _testQuestAmount; i++)
        {
            _testQuests.Add(new QuestProgress(_testQuest));
        }

        _activeQuests.Add(new QuestProgress(_startingQuest));
    }

    public void AcceptQuest(Quest quest)
    {
        if (quest == null) return;

        if (IsQuestActive(quest._questID)) return;
        if (IsQuestCompleted(quest._questID)) return;

        _activeQuests.Add(new QuestProgress(quest));
        _uiManager?.UpdateQuestUI();
    }

    public bool IsQuestActive(int questID)
    {
        return _activeQuests.Exists(q => q.QuestID == questID && !q.IsCompleted);
    }

    public bool IsQuestCompleted(int questID)
    {
        if (_completedQuestIds.Contains(questID)) return true;

        var progress = _activeQuests.Find(q => q.QuestID == questID);
        if (progress != null && progress.IsCompleted)
        {
            _completedQuestIds.Add(questID);
            _activeQuests.Remove(progress);
            _uiManager?.UpdateQuestUI();
            return true;
        }

        return false;
    }

    public void CompleteQuest(int questID)
    {
        var progress = _activeQuests.Find(q => q.QuestID == questID);
        Quest completedQuest = progress?.Quest;

        if (progress != null)
        {
            progress.MarkCompleted();
            _activeQuests.Remove(progress);
            _completedQuestIds.Add(questID);
            _uiManager?.UpdateQuestUI();
        }
        else
        {
            _completedQuestIds.Add(questID);
            _uiManager?.UpdateQuestUI();
        }

        if (completedQuest != null && completedQuest._nextQuest != null)
        {
            AcceptQuest(completedQuest._nextQuest);
            Debug.Log($"Next quest '{completedQuest._nextQuest._questTitle}' started automatically.");
        }

        if (completedQuest._givesKey)
        {
            ServiceHub.Instance.PlayerController._hasKey = true;
        }
    }

    public void AddProgress(int questID, int amount)
    {
        var progress = _activeQuests.Find(q => q.QuestID == questID);
        if (progress == null) return;

        progress.AddProgress(amount);

        if (progress.IsCompleted)
        {
            CompleteQuest(questID);
        }
        else
        {
            _uiManager?.UpdateQuestUI();
        }
    }
}