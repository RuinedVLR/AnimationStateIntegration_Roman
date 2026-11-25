using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Quest Dialogue")]
public class Dialogue : ScriptableObject
{
    public string npcName;

    public DialogueNode notStarted;
    public DialogueNode inProgress;
    public DialogueNode completed;

    public Quest quest;
}

[System.Serializable]
public class DialogueNode
{
    [TextArea(2, 8)]
    public string[] sentences;

    public bool givesQuest;
    public bool endsDialogue;

    public Quest quest;
}

