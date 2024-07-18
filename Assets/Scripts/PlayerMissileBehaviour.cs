using Unity.VisualScripting;
using UnityEngine;

public class PlayerMissileBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "InvaderMissile")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}