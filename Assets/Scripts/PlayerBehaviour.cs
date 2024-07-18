using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5.0f;
    [SerializeField] private GameObject MissilePrefab;
    [SerializeField] private float MissileSpeed = 5.0f;
    [SerializeField] private AudioClip ShootAudioClip;
    [SerializeField] private AudioClip DeathAudioClip;
    [SerializeField] private Sprite PlayerSprite;
    [SerializeField] private Sprite[] ExplosionSprites;
    [SerializeField] private int MaxMissiles = 1;
    [SerializeField] private InputAction PlayerMovement;
    [SerializeField] private InputAction PlayerFire;
    private Vector2 MoveDirection;
    private GameManagerBehaviour GameManager;
    private WaveBehaviour WaveManager;
    private int ExplosionCount = 0;
    private float DieAnimationDelay = 0.2f;
    private SpriteRenderer SpriteRenderer;
    private Collider2D Collider2D;


    //##########################
    // UNITY LIFECYCLE METHODS #
    //##########################

    void OnEnable()
    {
        PlayerMovement.Enable();
        PlayerFire.Enable();
    }

    void OnDisable()
    {
        PlayerMovement.Disable();
        PlayerFire.Disable();
    }

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            MoveDirection = PlayerMovement.ReadValue<Vector2>();
            if (PlayerFire.triggered) Fire();
            return;
        }
        MoveDirection = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("InvaderMissile"))
        {
            Destroy(other.gameObject);
            Die();
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(MoveDirection.x, 0, 0) * Time.deltaTime * MoveSpeed;
        float x = Mathf.Clamp(transform.position.x, GameConstants.LEFT_SCREEN_BORDER, GameConstants.RIGHT_SCREEN_BORDER);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    //#################
    // CUSTOM METHODS #
    //#################

    private GameManagerBehaviour GetGameManager()
    {
        return GameManager ?? (GameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>());
    }

    private WaveBehaviour GetWaveManager()
    {
        return GameObject.Find("Wave")?.GetComponent<WaveBehaviour>();
    }

    private void PlaySound(AudioClip audioClip)
    {
        AudioManager.Instance?.PlaySound(audioClip, GetGameManager().Volume);
    }

    private void Fire()
    {
        try
        {
            if (GameObject.FindGameObjectsWithTag("PlayerMissile").Length >= MaxMissiles || GetWaveManager().GetWaveState() != WaveState.Attacking) return;
            GameObject missile = Instantiate(MissilePrefab, transform.position, Quaternion.identity);
            missile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * MissileSpeed, ForceMode2D.Impulse);
            PlaySound(ShootAudioClip);
        }
        catch (System.NullReferenceException nre)
        {
            Debug.Log("WaveManger not found, probably waiting for the wave to appear");
        }
    }

    private void Die()
    {
        gameObject.tag = "Explosion";
        Collider2D.enabled = false;
        Invoke("DieAnimation", DieAnimationDelay);
        PlaySound(DeathAudioClip);
    }

    private void DieAnimation()
    {
        if (ExplosionCount++ > 8)
        {
            SpriteRenderer.sprite = PlayerSprite;
            Collider2D.enabled = true;
            gameObject.tag = "Player";
            ExplosionCount = 0;
            GetGameManager().DecrementLives();
            return;
        }
        SpriteRenderer.sprite = ExplosionSprites[ExplosionCount % ExplosionSprites.Length];
        Invoke("DieAnimation", DieAnimationDelay);
    }
}