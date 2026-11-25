using UnityEngine;

[System.Serializable]
public class QuestObjective
{
    public string _objectiveID;
    public string _description;
    public ObjectiveType _type;
    public int _requiredAmount;
    public int _currentAmount;
    public bool IsCompleter => _currentAmount >= _requiredAmount;
}
