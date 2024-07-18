using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InvaderBehaviour : MonoBehaviour
{
    [SerializeField] private int ScoreValue = 10;
    [SerializeField] private AudioClip ExplosionSound;
    [SerializeField] private GameObject MissilePrefab;
    [SerializeField] private float MissileSpeed = 5.0f;
    [SerializeField] private float InitialisationDelay = 0.1f;
    [SerializeField] private float InitialisationOrder = 1;
    [SerializeField] private Sprite ExplosionSprite;
    public Sprite[] Frames;
    private int CurrentFrame = 0;
    private WaveBehaviour WaveManager;
    private GameManagerBehaviour GameManager;

    public bool IsInitialised { get; private set; } = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Invoke("Initialisation", (InitialisationDelay * InitialisationOrder));
     }

    private GameManagerBehaviour GetGameManager()
    {
        if(GameManager == null) 
        {
            GameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        }
        return GameManager;
    }

    private WaveBehaviour GetWaveManager()
    {
        if(WaveManager == null) 
        {
            WaveManager = GameObject.Find("Wave").GetComponent<WaveBehaviour>();
        }
        return WaveManager;
    }

    void Initialisation()
    {    
        GetComponent<SpriteRenderer>().enabled = true;
        IsInitialised = true;
    }

    public void Animate()
    {
        if(gameObject.tag != "Explosion")
        {
            GetComponent<SpriteRenderer>().sprite = Frames[CurrentFrame++ % Frames.Length];
        } 
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(MissilePrefab, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.down * MissileSpeed, ForceMode2D.Impulse);
    }

    // Trigger on bullet collision
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
        GetWaveManager().IncreaseSpeed();
        SetExplosion();
        DestroyInvaderOrColumn();
    }

    private void SetExplosion()
    {
        if(AudioManager.Instance != null) AudioManager.Instance.PlaySound(ExplosionSound, 1.0f);
        GetComponent<SpriteRenderer>().sprite = ExplosionSprite;
        Destroy(GetComponent<BoxCollider2D>());
        gameObject.tag = "Explosion";
    }

    private void DestroyInvaderOrColumn()
    {
        int invaderCount = 0;
        foreach(Transform invader in transform.parent)
        {
            if(invader.tag == "Invader") invaderCount++;
        }
        if(invaderCount == 0)
        {
            Invoke("DestroyColumn", 0.25f);
        }
        else 
        {   
            Invoke("DestroyInvader", 0.25f);
        }
    }

    private void DestroyColumn()
    {
        GetWaveManager().CheckForEndOfWave();
        Destroy(transform.parent.gameObject);
    }

    private void DestroyInvader()
    {
        Destroy(gameObject);
    }

}
