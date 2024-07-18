using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MysteryShipBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip MysteryShipSound;
    [SerializeField] private AudioClip MysteryShipExplosionSound;
    [SerializeField] private Sprite MysteryShipExplosion;
    [SerializeField] private int ScoreValue = 200;
    private GameManagerBehaviour GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GetAudioManager().PlaySound(MysteryShipSound, 1.0f);
    }
    
    private GameManagerBehaviour GetGameManager()
    {
        if(GameManager == null) 
        {
            GameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        }
        return GameManager;
    }

    private AudioManager GetAudioManager()
    {
        return AudioManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerMissile")
        {
            Destroy(other.gameObject);
            Die();
        }
    }

    private void Die()
    {
        GetGameManager().AddScore(ScoreValue);
        GetAudioManager().PlaySound(MysteryShipExplosionSound, 1.0f);
        SetExplosion();
        DestroyMysteryShip();
    }

    private void SetExplosion()
    {
        GetComponent<SpriteRenderer>().sprite = MysteryShipExplosion;
        Destroy(GetComponent<BoxCollider2D>());
        gameObject.tag = "Explosion";
    }   

    private void DestroyMysteryShip()
    {
        Destroy(gameObject, 0.25f);
    }
}
