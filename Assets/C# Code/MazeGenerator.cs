// Sarah Hannah Langdon
// 2026-17-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=_aeYq5BmDMg

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Cell Prefab")]
    [SerializeField] private MazeCell _mazeCellPrefab;

    [Header("Maze Dimensions")]
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;

    [SerializeField] private float _cellSize = 5f;
    
    [Header("Coin Generation")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _coinCount = 10;

    [Header("NavMesh")]
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Header("Enemy Generation")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _enemyCount = 3;

    private MazeCell[,] _mazeGrid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinScore.ResetCoins();
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(
                    _mazeCellPrefab,
                    new Vector3(x * _cellSize, 0, z * _cellSize),
                    Quaternion.identity,
                    transform
                );
            }
        }

        // Generate the maze and remove walls
        GenerateMaze(null, _mazeGrid[0, 0]);

        // Build NavMesh after maze generation is complete
        navMeshSurface.BuildNavMesh();

        // Spawn collectibles
        SpawnCoins();
        SpawnEnemies();
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }

        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();

    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x / (int)_cellSize;
        int z = (int)currentCell.transform.position.z / (int)_cellSize;

        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }
        
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }

    }

    private void SpawnCoins()
    {
        List<MazeCell> availableCells = new List<MazeCell>();

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                // Skip the starting cell
                if (x == 0 && z == 0)
                    continue;

                availableCells.Add(_mazeGrid[x, z]);
            }
        }

        int coinsToSpawn = Mathf.Min(_coinCount, availableCells.Count);

        for (int i = 0; i < coinsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableCells.Count);

            MazeCell selectedCell = availableCells[randomIndex];

            Vector3 spawnPosition = selectedCell.transform.position
                + Vector3.up * 1f;

            Instantiate(_coinPrefab, spawnPosition,
                Quaternion.identity, transform);

            availableCells.RemoveAt(randomIndex);
        }
    }

    private void SpawnEnemies()
    {
        List<MazeCell> availableCells = new List<MazeCell>();

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                // Skip the starting cell
                if (x == 0 && z == 0)
                    continue;

                availableCells.Add(_mazeGrid[x, z]);
            }
        }

        int enemiesToSpawn = Mathf.Min(_enemyCount, availableCells.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableCells.Count);

            MazeCell selectedCell = availableCells[randomIndex];

            Vector3 spawnPosition = selectedCell.transform.position
                + Vector3.up * 1f;

            Instantiate(_enemyPrefab, spawnPosition,
                Quaternion.identity, transform);

            availableCells.RemoveAt(randomIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
