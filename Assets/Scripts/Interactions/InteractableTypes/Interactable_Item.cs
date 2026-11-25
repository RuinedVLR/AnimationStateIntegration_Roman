using UnityEngine;

public class Interactable_Item : MonoBehaviour, IInteractable
{
    private QuestManager _questManager;
    private DialogueManager _dialogueManager;

    public Dialogue _notInteractableDialogue;
    public enum ItemType
    {
        Pancake,
        Key
    }
    public ItemType _itemType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _questManager = ServiceHub.Instance.QuestManager;
        _dialogueManager = ServiceHub.Instance.DialogueManager;
    }

    public void Interact()
    {
        if (_itemType == ItemType.Pancake)
        {
            if (_questManager._activeQuests.Count > 0 && _questManager._activeQuests[0].ObjectiveType == ObjectiveType.FindPancakes)
            {
                _questManager.AddProgress(0, 1);
                Destroy(gameObject);
            }
            else if (_questManager._activeQuests[0].ObjectiveType != ObjectiveType.FindPancakes)
            {
                _dialogueManager.StartDialogue(_notInteractableDialogue);
                return;
            }
        }
    }
}
