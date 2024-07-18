using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderIntroScript : MonoBehaviour
{
    [SerializeField] private Sprite[] SpriteFrames;
    private int CurrentFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(SpriteFrames.Length > 1)
        {
            InvokeRepeating("Animate", 0.0f, 0.5f);
        }
    }

    public void Animate()
    {
        GetComponent<SpriteRenderer>().sprite = SpriteFrames[CurrentFrame % SpriteFrames.Length];
        CurrentFrame++;
    }
}
