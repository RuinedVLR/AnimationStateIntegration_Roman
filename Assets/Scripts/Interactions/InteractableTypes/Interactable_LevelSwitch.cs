using UnityEngine;

public class Interactable_LevelSwitch : MonoBehaviour, IInteractable
{
    private LevelManager _levelManager;
    private LevelSwitch _levelSwitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _levelManager = ServiceHub.Instance.LevelManager;
        _levelSwitch = GetComponent<LevelSwitch>();
    }

    public void Interact()
    {
        _levelManager.LevelChange(_levelSwitch._levelToActivate, _levelSwitch._spawnLocation);
    }
}
