using UnityEngine;

public class MissileBoundsBehaviour : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}