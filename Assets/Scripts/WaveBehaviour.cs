using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

//enum for state
public enum WaveState
{
    Initialising,
    Attacking,
    AllDead,
    Paused
}

public class WaveBehaviour : MonoBehaviour
{
    [SerializeField] public float StartHeight = 0.0f;
    [SerializeField] public float Speed = 0.75f;
    [SerializeField] public float SpeedIncrease = 1.05f;
    [SerializeField] public float AnimationDelay = 1.0f;
    [SerializeField] public float FiringDelay = 2.0f;
    [SerializeField] public AudioClip[] InvaderSounds;
    private int soundIndex = 0;
    private GameManagerBehaviour GameManager;


    private float Direction = 1.0f;
    private WaveState State = WaveState.Initialising;

    void Start()
    {
        Invoke("AnimateInvaders", AnimationDelay);
        State = WaveState.Initialising;
        WaitForInitialisation();
    }

    public WaveState GetWaveState()
    {
        return State;
    }


    private GameManagerBehaviour GetGameManager()
    {
        return GameManager ?? (GameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>());
    }

    private void PlayWaveSound()
    {
        AudioManager.Instance?.PlaySound(InvaderSounds[soundIndex++ % InvaderSounds.Length], GetGameManager().Volume/2);
    }

    public void WaitForInitialisation()
    {
        State = WaveState.Initialising;
        GameObject[] Invaders = GameObject.FindGameObjectsWithTag("Invader");
        foreach (GameObject Invader in Invaders)
        {
            InvaderBehaviour invaderBehaviour = Invader.GetComponent<InvaderBehaviour>();
            if (!invaderBehaviour.IsInitialised)
            {
                Invoke("WaitForInitialisation", 0.1f);
                return;
            }
        }
        State = WaveState.Attacking;
        Invoke("Firing", FiringDelay);
    }

    void Firing()
    {
        GameObject[] Columns = GameObject.FindGameObjectsWithTag("Column");
        foreach (GameObject Column in Columns)
        {
            Transform lastInvader = Column.transform.GetChild(Column.transform.childCount - 1);
            InvaderBehaviour invaderBehaviour = lastInvader.GetComponent<InvaderBehaviour>();
            int FiringRange = Mathf.CeilToInt(Columns.Length / 3);
            if (Random.Range(0, FiringRange) == 0)
            {
                invaderBehaviour.Fire();
            }
        }
        Invoke("Firing", FiringDelay);
    }

    void AnimateInvaders()
    {
        PlayWaveSound();
        GameObject[] Invaders = GameObject.FindGameObjectsWithTag("Invader");
        foreach (GameObject Invader in Invaders)
        {
            Invader.GetComponent<InvaderBehaviour>().Animate();
        }
        Invoke("AnimateInvaders", AnimationDelay);
    }

    public void SetDirection(float direction)
    {
        Direction = direction;
    }

    public void ReverseDirection()
    {
        Direction *= -1;
        transform.position += new Vector3(0, -0.1f, 0);
    }

    public void IncreaseSpeed()
    {
        Speed *= SpeedIncrease;
        AnimationDelay /= SpeedIncrease;
    }

    public void CheckForEndOfWave()
    {
        if (transform.childCount <= 1) // the last column takes 0.25 seconds to be destroyed
        {
            State = WaveState.AllDead;
            var gameManagerBehaviour = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
            if (gameManagerBehaviour != null) gameManagerBehaviour.NextWave();
        }
    }

    void Update()
    {
        if (State == WaveState.Attacking) transform.position += new Vector3(Speed * Direction * Time.deltaTime, 0, 0);
    }
}