using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    public bool isAliveNextGeneration  = false;

    int neighborCount = 0;

    int defaultTrailFadeTime = 20;
    int trailFadeTime;

    int defaultTrailDeathTime = 10;
    int trailDeathTime;

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

    private void RandomizeAliveState()
    {
        if (Random.Range(0, 10) == 0)
        {
            isAlive = true;
        }
    }

    public void UpdateLife()
    {
        bool startedGenerationAlive = isAlive;

        isAlive = isAliveNextGeneration;
        if (isAlive != startedGenerationAlive)
        {
            trailFadeTime = defaultTrailFadeTime;
            trailDeathTime = defaultTrailDeathTime;
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
        else if (trailFadeTime == defaultTrailFadeTime)
        {
            trailColor = trailStartColor;
            sprite.color = trailColor;

            trailFadeTime--;
        }
        else if (trailFadeTime > 0)
        {
            float trailFadeRate = (float)trailFadeTime / defaultTrailFadeTime;
            sprite.color = Color.Lerp(trailEndColor, trailStartColor, trailFadeRate);

            trailFadeTime--;
        }
        else if (trailDeathTime > 0)
        {
            float trailFadeRate = (float)trailDeathTime / defaultTrailDeathTime;
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
        switch(neighborCount)
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
            isAlive = !isAlive;
            UpdateColor();
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
            isAlive = !isAlive;
            UpdateColor();
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
}