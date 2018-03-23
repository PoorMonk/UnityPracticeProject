using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController _instance = null;
    private BoardManager boardManager;
    public int level = 3;

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = false;

	// Use this for initialization
	void start () {
        boardManager = GetComponent<BoardManager>();
		if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        
        boardManager.SetupScene(level);
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver()
    {
        enabled = false;
    }
}
