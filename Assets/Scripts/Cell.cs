using UnityEngine;

public class Cell : MonoBehaviour
{
    SpriteRenderer sr;

    public bool isAlive;
    public bool isAliveNextGeneration = false;

    public int neighborCount = 0;
    public int trailFadeTime;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }


    public SpriteRenderer GetSpriteRenderer()
    {
        return sr;
    }
}