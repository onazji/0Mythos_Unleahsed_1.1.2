using UnityEngine;

public class SunDrift : MonoBehaviour
{
    public float speed = 5f;
    void Update()
    {
        transform.Rotate(Vector3.right, speed * Time.deltaTime);
    }
}
