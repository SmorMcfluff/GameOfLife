using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    public bool isAliveNextGeneration  = false;

    private int neighborCount = 0;

    public int maxTrailFadeTime = 30;
    public int trailFadeTime;

    public int secondGradientStartTime = 10;

    SpriteRenderer sprite;
    Color aliveColor = Color.white;

    Color trailStartColor = Color.yellow;
    Color trailMidColor = Color.red;
    Color trailEndColor = Color.black;

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
        }

        isAliveNextGeneration = false;

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
        else if (trailFadeTime > secondGradientStartTime)
        {
            float trailFadeRate = (float)trailFadeTime / maxTrailFadeTime;
            sprite.color = Color.Lerp(trailMidColor, trailStartColor, trailFadeRate);

            trailFadeTime--;
        }
        else if (trailFadeTime > 0)
        {
            float trailFadeRate = (float)trailFadeTime / secondGradientStartTime;
            sprite.color = Color.Lerp(trailEndColor, trailMidColor, trailFadeRate);

            trailFadeTime--;
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


    public void HardKillCell()
    {
        isAlive = false;
        trailFadeTime = 0;
        UpdateColor();
    }


    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
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
}