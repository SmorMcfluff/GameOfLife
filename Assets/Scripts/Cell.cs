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


    private void ChangeColorOnHoverOver()
    {
        sprite.enabled = true;
        sprite.color = Color.yellow;
    }


    private void DrawCell()
    {
        if (GameOfLife.deleteMode)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }
        ColorManager.UpdateColor(this);
    }


    private void OnMouseOver()
    {
        if (GameOfLife.isPaused)
        {
            ChangeColorOnHoverOver();
        }

        if (Input.GetMouseButtonDown(0) && GameOfLife.isPaused)
        {
            GameOfLife.SetEditMode(isAlive);
            DrawCell();
        }
    }


    private void OnMouseEnter()
    {
        if (GameOfLife.isPaused)
        {
            ChangeColorOnHoverOver();
        }

        if (Input.GetMouseButton(0) && GameOfLife.isPaused)
        {
            DrawCell();
        }
    }


    private void OnMouseExit()
    {
        ColorManager.UpdateColor(this);
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return sprite;
    }
}