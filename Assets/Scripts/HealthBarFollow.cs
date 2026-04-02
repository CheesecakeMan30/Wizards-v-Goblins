using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}