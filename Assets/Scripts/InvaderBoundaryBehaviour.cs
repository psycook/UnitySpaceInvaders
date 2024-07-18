using Unity.VisualScripting;
using UnityEngine;

public class InvaderBoundaryBehaviour : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Column")
        {
            GameObject.Find("Wave").GetComponent<WaveBehaviour>().ReverseDirection();
        }
        if(other.gameObject.tag == "MysteryShip")
        {
            Destroy(other.gameObject);
        }
    }
}