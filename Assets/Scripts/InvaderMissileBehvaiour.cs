using UnityEngine;

public class InvaderMissileBehaviour : MonoBehaviour
{
    [SerializeField] private float AnimationSpeed = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("FlipMissile", AnimationSpeed, AnimationSpeed);
    }

    private void FlipMissile()
    {
        GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
    }
}
