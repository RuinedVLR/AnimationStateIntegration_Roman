using UnityEngine;

public class Interactable_WinTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue _notInteractableDialogue;

    public void Interact()
    {
        if(ServiceHub.Instance.PlayerController._hasKey)
            ServiceHub.Instance.GameStateManager.SetState(GameState.Win);
        else if (_notInteractableDialogue != null)
            ServiceHub.Instance.DialogueManager.StartDialogue(_notInteractableDialogue);
    }
}
