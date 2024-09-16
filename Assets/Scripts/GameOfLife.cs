using System.Runtime.CompilerServices;
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

    public float timerMax;
    float timer;
    

    void Start()
    {
        Application.targetFrameRate = 0;
        
        mainCamera = Camera.main;
        mainCamera.orthographicSize = 5;
        mainCamera.aspect = 2;

        screenHeight = mainCamera.orthographicSize * 2;

        gridHeight = 200;
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            EmptyGrid();
        }

        if (Input.GetKeyDown(KeyCode.N) && isPaused)
        {
            NextGeneration();
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

                Vector2 spawnPosition = CellPosition(cellSize, x, y);
                cellGrid[x, y].transform.position = spawnPosition;

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


    private Vector2 CellPosition(float cellSize, int x, int y)
    {
        return new Vector2(cellSize * x + (cellSize - 1) * 0.5f, cellSize * y + (cellSize - 1) * 0.5f);
    }
}