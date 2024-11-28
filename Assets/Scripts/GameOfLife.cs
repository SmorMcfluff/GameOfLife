using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    static Camera mainCamera;

    public Cell cell;
    Cell[,] cellGrid;

    readonly int lifeChancePercentage = 20;

    int gridHeight;
    int gridWidth;

    float screenHeight;

    float timerMax;
    float timer;

    static bool isPaused = false;

    Vector2 mousePos;

    void Start()
    {
        Application.targetFrameRate = 0;

        mainCamera = Camera.main;
        mainCamera.orthographicSize = 5;
        mainCamera.aspect = 2;

        screenHeight = mainCamera.orthographicSize * 2;

        gridHeight = 300;
        gridWidth = gridHeight * 2;

        timerMax = 0.02f;
        timer = 0;

        GenerateGrid();
        RandomizeGrid();
    }


    void Update()
    {
        PlayerInputs();

        if (timer >= timerMax && !isPaused)
        {
            NextGeneration();
            timer = 0;
        }
        if (!isPaused)
        {
            timer += Time.deltaTime;
        }
    }


    void GenerateGrid()
    {
        float cellSize = screenHeight / gridHeight;

        cellGrid = new Cell[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                cellGrid[x, y] = Instantiate(cell);
                Cell currentCell = cellGrid[x, y];

                currentCell.transform.localScale *= cellSize;
                currentCell.transform.position = CalculateCellPosition(cellSize, x, y);
            }
        }
    }


    void NextGeneration()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                CheckCellNeighbors(x, y);
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                UpdateCellLife(x, y);
            }
        }
    }


    void CheckCellNeighbors(int currentCellX, int currentCellY)
    {
        Cell currentCell = cellGrid[currentCellX, currentCellY];

        currentCell.neighborCount = 0;

        for (int x = currentCellX - 1; x <= currentCellX + 1; x++)
        {
            int xToCheck = x;
            if (x < 0 || x >= gridWidth)
            {
                xToCheck = Mod(x, gridWidth);
            }

            for (int y = currentCellY - 1; y <= currentCellY + 1; y++)
            {
                int yToCheck = y;
                if (y < 0 || y >= gridHeight)
                {
                    yToCheck = Mod(y, gridHeight);
                }
                if (x == currentCellX && y == currentCellY)
                {
                    continue;
                }
                if (cellGrid[xToCheck, yToCheck].isAlive)
                {
                    currentCell.neighborCount++;
                }
            }
        }

        currentCell.isAliveNextGeneration = SetCellNextGenerationStatus(currentCell);
    }

    int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }


    bool SetCellNextGenerationStatus(Cell currentCell)
    {
        switch (currentCell.neighborCount)
        {
            default:
                return false;
            case 2:
                return currentCell.isAlive;
            case 3:
                return true;
        }
    }

    void UpdateCellLife(int x, int y)
    {
        Cell currentCell = cellGrid[x, y];
        bool startedGenerationAlive = currentCell.isAlive;

        currentCell.isAlive = currentCell.isAliveNextGeneration;

        if (currentCell.isAlive != startedGenerationAlive)
        {
            currentCell.trailFadeTime = ColorManager.maxTrailFadeTime;
        }

        currentCell.isAliveNextGeneration = false;

        ColorManager.UpdateColor(currentCell);
    }

    void PlayerInputs()
    {
        if (Input.GetMouseButton(0) ^ Input.GetMouseButton(1))
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            DrawCell(Input.GetMouseButton(0));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EmptyGrid();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextGeneration();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EmptyGrid();
            RandomizeGrid();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int x = Random.Range(0, gridWidth);

            DrawColumn(x);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DrawColumn(Mathf.CeilToInt(gridWidth * 0.5f));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int y = Random.Range(0, gridHeight);

            DrawRow(y);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DrawRow(Mathf.CeilToInt(gridHeight / 2));
        }
    }


    void PauseGame()
    {
        isPaused = !isPaused;
        timer = 0;
    }


    void EmptyGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Cell currentCell = cellGrid[x, y];
                cellGrid[x, y].isAlive = false;
                currentCell.trailFadeTime = 0;
                ColorManager.UpdateColor(currentCell);
            }
        }
    }


    void RandomizeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (Random.Range(0, 101) <= lifeChancePercentage)
                {
                    Cell currentCell = cellGrid[x, y];

                    currentCell.isAlive = true;
                    ColorManager.UpdateColor(currentCell);
                }
            }
        }
    }


    void DrawCell(bool leftClick)
    {
        int[] closestCell = FindClosestCell();
        Cell clickedCell = cellGrid[closestCell[0], closestCell[1]];

        if (leftClick)
        {
            clickedCell.isAlive = true;
        }
        else
        {
            clickedCell.isAlive = false;
            clickedCell.trailFadeTime = 0;
        }

        ColorManager.UpdateColor(clickedCell);
    }


    void DrawColumn(int x)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            Cell currentCell = cellGrid[x, y];

            currentCell.isAlive = true;
            ColorManager.UpdateColor(currentCell);
        }
    }


    void DrawRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Cell currentCell = cellGrid[x, y];

            currentCell.isAlive = true;
            ColorManager.UpdateColor(currentCell);
        }
    }


    Vector2 CalculateCellPosition(float cellSize, int x, int y)
    {
        return new Vector2(cellSize * x + (cellSize - 1) * 0.5f, cellSize * y + (cellSize - 1) * 0.5f);
    }


    int[] FindClosestCell()
    {
        float[] xDistances = new float[gridWidth];
        float[] yDistances = new float[gridHeight];

        int closestX = 0;
        int closestY = 0;

        for (int i = 0; i < gridWidth; i++)
        {
            float distanceToMouse = Mathf.Abs(mousePos.x - cellGrid[i, 0].transform.position.x);
            xDistances[i] = distanceToMouse;

            if (i > 0)
            {
                if (xDistances[i] > xDistances[i - 1])
                {
                    closestX = (i - 1);
                    break;
                }
            }
        }

        for (int i = 0; i < gridHeight; i++)
        {
            float distanceToMouse = Mathf.Abs(mousePos.y - cellGrid[0, i].transform.position.y);
            yDistances[i] = distanceToMouse;

            if (i > 0)
            {
                if (yDistances[i] > yDistances[i - 1])
                {
                    closestY = (i - 1);
                    break;
                }
            }
        }

        int[] closestCellCoordinates = { closestX, closestY };
        return closestCellCoordinates;
    }
}