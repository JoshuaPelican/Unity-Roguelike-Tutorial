using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [System.Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    [Min(1)]
    public int rows = 8;
    [Min(1)]
    public int columns = 8;

    [Min(1)]
    public int tileSize;

    public Count wallCount = new Count(5, 9);
    public Count pickupCount = new Count(1, 4);

    public GameObject victoryTile;
    public GameObject[] roadTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] pickupTiles;
    public GameObject[] obstacleTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private void InitializeGrid()
    {
        for (int x = 1; x < columns-1; x++)
        {
            for (int y = 1; y < rows-1; y++)
            {
                gridPositions.Add(new Vector3(x * tileSize, y * tileSize, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns+1; x++)
        {
            for (int y = -1; y < rows+1; y++)
            {
                GameObject chosenTile = roadTiles[Random.Range(0, roadTiles.Length)];

                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                    chosenTile = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject tileInstance = Instantiate(chosenTile, new Vector3(x, y, 0f), RandomCardinalRotation);

                tileInstance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randIndex];
        gridPositions.RemoveAt(randIndex);

        return randomPosition;
    }

    private Quaternion RandomCardinalRotation
    {
        get { return Quaternion.Euler(0, 0, Random.Range(0, 3) * 90); }
    }

    private void LayoutTileAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randPosition = RandomPosition();

            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randPosition, RandomCardinalRotation);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeGrid();
        LayoutTileAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutTileAtRandom(pickupTiles, pickupCount.minimum, pickupCount.maximum);

        int obstacleCount = (int)Mathf.Log(level, 2f);
        LayoutTileAtRandom(obstacleTiles, obstacleCount, obstacleCount);

        Instantiate(victoryTile, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

    }
}
