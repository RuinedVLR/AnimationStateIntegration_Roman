using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject _current;

    [SerializeField] private GameObject _lvl1;
    [SerializeField] private Transform _player;

    private void Start()
    {
        _lvl1.SetActive(true);
        _current = _lvl1;
    }

    public void LevelChange(GameObject targetLevel, Transform spawnPoint)
    {
        if (targetLevel == null)
        {
            Debug.LogWarning("LevelChange: targetLevel is null.");
            return;
        }

        if (_current == targetLevel)
        {
            Debug.Log("LevelChange: already on target level.");
            return;
        }

        targetLevel.SetActive(true);

        if (spawnPoint != null && _player != null)
        {
            _player.position = spawnPoint.position;
        }

        if (_current != null)
        {
            _current.SetActive(false);
        }

        _current = targetLevel;
        Debug.Log($"LevelChange: switched to {targetLevel.name}");
    }
}
