using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public static Camera mainCamera;

    public Cell cell;

    Cell[,] cellGrid;
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


    void GenerateGrid()
    {
        float cellSize = screenHeight / gridHeight;

        cellGrid = new Cell[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                cellGrid[x, y] = Instantiate(cell);
                cellGrid[x, y].transform.localScale *= cellSize;

                cellGrid[x, y].transform.position = CellPosition(cellSize, x, y);

                cellGrid[x, y].UpdateColor();
            }
        }
    }


    void NextGeneration()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                cellGrid[x, y].CheckNeighbors(cellGrid, x, y, gridWidth, gridHeight);
            }
        }
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                cellGrid[x, y].UpdateLife();
            }
        }
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
                cellGrid[x, y].HardKillCell();
            }
        }
    }


    void RandomizeGrid()
    {
        EmptyGrid();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                cellGrid[x, y].RandomizeAliveState();
            }
        }
    }


    private void DrawColumn(int x)
    {
        for (int y = 0; y < gridHeight; y++)
        {

            cellGrid[x, y].isAlive = true;
            cellGrid[x, y].UpdateColor();
        }
    }
    

    private void DrawRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {

            cellGrid[x, y].isAlive = true;
            cellGrid[x, y].UpdateColor();
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