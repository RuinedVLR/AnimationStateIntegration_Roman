using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void LateUpdate()
    {
        if (_player == null) 
            return;

        var pos = transform.position;
        pos.x = _player.position.x;
        pos.y = _player.position.y;
        transform.position = pos;
    }
}
