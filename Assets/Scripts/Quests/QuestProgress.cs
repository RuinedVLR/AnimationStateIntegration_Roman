public class QuestProgress
{
    public Quest Quest { get; private set; }
    public int QuestID => Quest._questID;
    public int CurrentAmount { get; private set; }
    public bool IsCompleted { get; private set; }
    public ObjectiveType ObjectiveType => Quest._objectiveType;

    public QuestProgress(Quest quest)
    {
        Quest = quest;
        CurrentAmount = 0;
        IsCompleted = false;
    }

    public void AddProgress(int amount)
    {
        if (IsCompleted) return;
        CurrentAmount += amount;
        if (Quest != null && CurrentAmount >= Quest._targetAmount)
            IsCompleted = true;
    }

    public void MarkCompleted()
    {
        IsCompleted = true;
    }
}