using UnityEngine;

public class Cell : MonoBehaviour
{
    SpriteRenderer sprite;

    public bool isAlive;
    public bool isAliveNextGeneration = false;

    public int neighborCount = 0;
    public int trailFadeTime;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }


    public SpriteRenderer GetSpriteRenderer()
    {
        return sprite;
    }
}