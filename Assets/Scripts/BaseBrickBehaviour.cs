using UnityEngine;

public class BaseBrickBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        if(other.gameObject.tag == "PlayerMissile" || other.gameObject.tag == "InvaderMissile")
        {
            Destroy(other.gameObject);
        }
    }
}