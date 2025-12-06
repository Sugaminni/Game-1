using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Radius around the spawn point within which to spawn
    public float radius = 1.5f;
    public Color gizmoColor = new Color(0.2f, 0.8f, 1f, 0.35f);

    public Vector3 GetRandomPoint()
    {
        var r = Random.insideUnitCircle * radius;
        return transform.position + new Vector3(r.x, 0f, r.y);
    }

    // Draw gizmos in the editor to visualize spawn point and radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, 0.15f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
