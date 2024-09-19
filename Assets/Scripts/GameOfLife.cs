using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    static Camera mainCamera;

    public Cell cell;
    public Cell[,] cellGrid;
    
    int lifeChancePercentage = 20;

    int gridHeight;
    int gridWidth;

    float screenHeight;
    float screenWidth;

    public static bool isPaused;

    float timerMax;
    float timer;
    
    public static bool deleteMode = false;

    void Start()
    {
        Application.targetFrameRate = 0;
        
        mainCamera = Camera.main;
        mainCamera.orthographicSize = 5;
        mainCamera.aspect = 2;

        screenHeight = mainCamera.orthographicSize * 2;

        gridHeight = 250;
        gridWidth = gridHeight * 2;

        isPaused = false;

        timerMax = 0.02f;
        timer = 0;

        GenerateGrid();
        RandomizeGrid();
    }


    void Update()
    {
        playerInputs();

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
                currentCell.transform.position = CellPosition(cellSize, x, y);
            }
        }
    }


    void playerInputs()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EmptyGrid();
        }

        if (Input.GetKeyDown(KeyCode.N) && isPaused)
        {
            NextGeneration();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            EmptyGrid();
            RandomizeGrid();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isPaused)
        {
            int x = Random.Range(0, gridWidth);

            DrawColumn(x);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isPaused)
        {
            DrawColumn(gridWidth/2);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && isPaused)
        {
            int y = Random.Range(0, gridHeight);

            DrawRow(y);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && isPaused)
        {
            DrawRow(gridHeight/2);
        }

        if (Input.GetMouseButtonUp(0))
        {
            deleteMode = false;
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

    public void CheckCellNeighbors(int currentCellX, int currentCellY)
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

        currentCell.isAliveNextGeneration = CellIsAliveNextGeneration(currentCell);
    }


    private int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }


    bool CellIsAliveNextGeneration(Cell currentCell)
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


    void PauseGame()
    {
        isPaused = !isPaused;
        timer = 0;
    }


    void EmptyGrid()
    {
        for (int x = 0; x  < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Cell currentCell = cellGrid[x, y];
                cellGrid[x,y].isAlive = false;
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


    private void DrawColumn(int x)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            Cell currentCell = cellGrid[x, y];

            currentCell.isAlive = true;
            ColorManager.UpdateColor(currentCell);
        }
    }
    

    private void DrawRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Cell currentCell = cellGrid[x, y];

            currentCell.isAlive = true;
            ColorManager.UpdateColor(currentCell);
        }
    }


    private Vector2 CellPosition(float cellSize, int x, int y)
    {
        return new Vector2(cellSize * x + (cellSize - 1) * 0.5f, cellSize * y + (cellSize - 1) * 0.5f);
    }


    public static void SetEditMode(bool isAlive)
    {
        deleteMode = isAlive;
    }
}