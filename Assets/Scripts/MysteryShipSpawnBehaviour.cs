using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryShipSpawnBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject MysteryShip;
    [SerializeField] private float MysteryShipDelay = 2.0f;
    [SerializeField] private int SpawnChance = 2;
    [SerializeField] private float MysteryShipSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMysteryShip", 0.0f, MysteryShipDelay);
    }

    void SpawnMysteryShip()
    {
        // Randomly spawn the mystery ship
        if (Random.Range(0, SpawnChance) == 0)
        {
            GameObject mysteryShip = Instantiate(MysteryShip, new Vector3(0, 0, 0), Quaternion.identity);
            mysteryShip.transform.SetParent(transform, false);
            mysteryShip.GetComponent<Rigidbody2D>().AddForce(Vector2.right * MysteryShipSpeed, ForceMode2D.Impulse);
        }

    }
}
