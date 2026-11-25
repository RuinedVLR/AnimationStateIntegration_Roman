using UnityEngine;

public class LevelSwitch : MonoBehaviour
{
    LevelManager _levelManager;

    public GameObject _levelToActivate;
    public Transform _spawnLocation;

    private void Start()
    {
        _levelManager = ServiceHub.Instance.LevelManager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !this.CompareTag("Interactable"))
        {
            Debug.Log("Player has entered the switch trigger area.");
            _levelManager.LevelChange(_levelToActivate, _spawnLocation);
        }
    }
}
