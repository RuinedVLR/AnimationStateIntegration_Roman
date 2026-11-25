using UnityEngine;

public class Interactable_Dialogue : MonoBehaviour, IInteractable
{
    private QuestManager _questManager;

    public Dialogue _dialogue;
    public Dialogue _notInteractableDialogue;
    private DialogueManager _dialogueManager;

    private void Start()
    {
        _questManager = ServiceHub.Instance.QuestManager;
        if (_questManager == null)
            Debug.LogError("QuestManager not found in ServiceHub.");

        _dialogueManager = ServiceHub.Instance.DialogueManager;
        if (_dialogueManager == null)
            Debug.LogError("DialogueManager not found in ServiceHub.");
    }

    public void Interact()
    {
        if (_dialogueManager == null || _dialogue == null) return;

        if (_dialogue.quest != null)
        {
            bool hasRelevantQuest = _questManager._activeQuests.Exists(q =>
                q.Quest.objectiveTargetType == _dialogue.quest.objectiveTargetType &&
                !q.IsCompleted);

            if (hasRelevantQuest)
            {
                _dialogueManager.StartDialogue(_dialogue);
            }
            else
            {
                if (_notInteractableDialogue != null)
                {
                    _dialogueManager.StartDialogue(_notInteractableDialogue);
                }
                else
                {
                    _dialogueManager.StartDialogue(_dialogue);
                }
            }
        }
        else
        {
            _dialogueManager.StartDialogue(_dialogue);
        }
    }
}