using UnityEngine;

public class Gizmo_SpawnPoint : MonoBehaviour
{
    // This method is called by Unity to draw the gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;   // You can change the color here
        Gizmos.DrawWireSphere(transform.position, 0.25f); // Draw a sphere at the spawn point location with radius 0.25
    }
}
