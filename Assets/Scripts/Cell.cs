using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    public bool isAliveNextGeneration  = false;

    private int neighborCount = 0;

    public int maxTrailFadeTime = 20;
    public int trailFadeTime;

    public int maxTrailDeathTime = 10;
    public int trailDeathTime;

    SpriteRenderer sprite;
    Color aliveColor = Color.white;
    Color deadColor = Color.black;

    Color trailStartColor = Color.yellow;
    Color trailEndColor = Color.red;
    Color trailColor;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        RandomizeAliveState();
    }


    public void RandomizeAliveState()
    {
        if (Random.Range(0, 10) == 0)
        {
            isAlive = true;
        }

        UpdateColor();
    }


    public void UpdateLife()
    {
        bool startedGenerationAlive = isAlive;

        isAlive = isAliveNextGeneration;
        if (isAlive != startedGenerationAlive)
        {
            trailFadeTime = maxTrailFadeTime;
            trailDeathTime = maxTrailDeathTime;
        }

        isAliveNextGeneration = false;

        UpdateColor();
    }


    public void HardKillCell()
    {
        isAlive = false;
        trailFadeTime = 0;
        trailDeathTime = 0;
        UpdateColor();
    }


    public void UpdateColor()
    {
        if (isAlive)
        {
            if (!sprite.enabled)
            {
                sprite.enabled = true;
            }
            sprite.color = aliveColor;
            return;
        }
        else if (trailFadeTime == maxTrailFadeTime)
        {
            trailColor = trailStartColor;
            sprite.color = trailColor;

            trailFadeTime--;
        }
        else if (trailFadeTime > 0)
        {
            float trailFadeRate = (float)trailFadeTime / maxTrailFadeTime;
            sprite.color = Color.Lerp(trailEndColor, trailStartColor, trailFadeRate);

            trailFadeTime--;
        }
        else if (trailDeathTime > 0)
        {
            float trailFadeRate = (float)trailDeathTime / maxTrailDeathTime;
            sprite.color = Color.Lerp(deadColor, trailEndColor, trailFadeRate);

            trailDeathTime--;
        }
        else
        {
            sprite.enabled = false;
        }
    }


    public void CheckNeighbors(Cell[,] grid, int currentCellX, int currentCellY, int maxX, int maxY)
    {
        neighborCount = 0;
        for (int x = currentCellX - 1; x <= currentCellX + 1; x++)
        {
            int xToCheck = x;
            if (x < 0 || x >= maxX)
            {
                xToCheck = Mod(x, maxX);
            }
            for (int y = currentCellY - 1; y <= currentCellY + 1; y++)
            {
                int yToCheck = y;
                if (y < 0 || y >= maxY)
                {
                    yToCheck = Mod(y, maxY);
                }
                if (x == currentCellX && y == currentCellY)
                {
                    continue;
                }
                if (grid[xToCheck, yToCheck].isAlive)
                {
                    neighborCount++;
                }
            }
        }

        isAliveNextGeneration = IsAliveNextGeneration();
    }


    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }


    private bool IsAliveNextGeneration()
    {
        switch (neighborCount)
        {
            default: 
                return false;
            case 2:
                return isAlive;
            case 3:
                return true;
        }
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
        UpdateColor();
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
        UpdateColor();
    }
}