using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    [Serializable]
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

    public GameObject Player;
    public GameObject ExitPoint;
    public GameObject[] wallTiles;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outWallTiles;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    public int row = 8;
    public int col = 8;

    //private GameObject Board;

    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < col; x++)
        {
            for (int y = 1; y < row; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < col + 1; ++x)
        {
            for (int y = -1; y < row + 1; ++y)
            {
                GameObject go = floorTiles[Random.Range(0, floorTiles.Length)];
                if (-1 == x || col == x || -1 == y || row == y)
                    go = outWallTiles[Random.Range(0, outWallTiles.Length)];
                GameObject instance = Instantiate(go, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.parent = boardHolder;
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); //把随机的格子移除，保证下次随机位置不会重复
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);
        for (int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(ExitPoint, new Vector3(col - 1, row - 1, 0f), Quaternion.identity);
        //Instantiate(ExitPoint, new Vector3(2, 2, 0f), Quaternion.identity);
    }

    private void Start()
    {
       // SetupScene(1);
    }

    /*
    void CreateWallAndFloor()
    {
        for (int i = -1; i < row + 1; i++)
        {
            for (int j = -1; j < col + 1; j++)
            {
                GameObject go;
                if ((i == -1) || (i == row )|| (j == -1) || (j == col))
                {
                    go = Instantiate(walls[Random.Range(0, walls.Length)], new Vector3(j, i, 0.0f), Quaternion.identity);
                    
                }
                else
                {
                    go = Instantiate(floors[Random.Range(0, floors.Length)], new Vector3(j, i, 0.0f), Quaternion.identity);
                }
                go.transform.SetParent(Board.transform);
            }
        }
    }

    void CreatePlayerAndExit()
    {
        Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(ExitPoint, new Vector3(col - 1, row - 1, 0.0f), Quaternion.identity);
    }

	// Use this for initialization
	void Start () {
        Board = new GameObject();
        Board.gameObject.name = "Board";
        CreateWallAndFloor();
        CreatePlayerAndExit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    */
}
