using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public int _questID;
    public string _questTitle;
    public int _targetAmount;
    public string _targetName;
    public Quest _nextQuest;
    public ObjectiveType _objectiveType;
    public ObjectiveTargetType objectiveTargetType;

    public bool _givesKey;
}

public enum ObjectiveType
{
    FindPancakes,
    Defeat,
    Explore,
    Talk
}

public enum ObjectiveTargetType
{
    Item,
    Mage,
    Slime,
    Sam,
    Door
}