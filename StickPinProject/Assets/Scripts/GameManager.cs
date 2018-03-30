using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static public GameManager instance = null;

    private Transform StartPoint;
    private Transform SpawnPoint;
    private Camera mainCamera;

    public GameObject Spin;
    private Pin spawnPin;
    public Text m_scoreText;
    int m_iCount = 0;
    public int level = 1;
    private int m_targetCount = 3;

    private bool IsGameOver = false;
    public float m_speed = 3f;
    private CircleRotate m_CircleRotate;
    private Text m_InfoText;
    private Text m_PassText;
    private Text m_OverText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void InitResource()
    {
        level = 1;
        m_targetCount = 3;
        m_PassText.text = "";
        //m_OverText.text = "";
    }

    private void OnDisable()
    {
       level = 1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_targetCount += level;       
        InitGame();
        m_CircleRotate.RotateSpeed += level * 10;
        //InitResource();
        //Debug.Log("level:" + level.ToString() + ", target:" + m_targetCount.ToString() + ", RotateSpeed:" + m_CircleRotate.RotateSpeed);
        level++;
    }

    private void InitGame()
    {
        StartPoint = GameObject.Find("StartPoint").transform;
        SpawnPoint = GameObject.Find("SpawnPoint").transform;
        m_CircleRotate = GameObject.Find("Disk").GetComponent<CircleRotate>();
        m_scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        m_InfoText = GameObject.Find("InfoText").GetComponent<Text>();
        m_PassText = GameObject.Find("PassText").GetComponent<Text>();
        m_OverText = GameObject.Find("OverText").GetComponent<Text>();
        SpawnPin();
        mainCamera = Camera.main;       
        IsGameOver = false;
        m_iCount = 0;
        m_scoreText.text = m_iCount.ToString();
        m_InfoText.text = "level:" + level.ToString() + " target:" + m_targetCount.ToString();
    }

    private void SpawnPin()
    {
        spawnPin = Instantiate(Spin, SpawnPoint.position, Spin.transform.rotation).GetComponent<Pin>();
    }
	
	// Update is called once per frame
	void Update () {
        if (IsGameOver) return;
		if (Input.GetMouseButtonDown(0))
        {
            if (spawnPin.IsReachStartPoint && !spawnPin.IsGotoCircle)
            {
                spawnPin.fly();
                
            }            
        }
	}

    public void ReachCircle()
    {
        SpawnPin();
        m_iCount++;

        m_scoreText.text = m_iCount.ToString();
        if (m_targetCount <= m_iCount)
        {
            GotoNextLevel();
        }
    }

    private void GotoNextLevel()
    {
        m_PassText.text = "PASS";
        GameOver(Color.yellow);
    }

    public void GameOver(Color col)
    {
        if (IsGameOver) return;
        if (col == Color.red)
            m_OverText.text = "OVER";
        IsGameOver = true;
        m_CircleRotate.enabled = false;
        spawnPin.Stopfly();
        StartCoroutine(GameOverAnimation(col));
    }

    IEnumerator GameOverAnimation(Color col)
    {
        while(true)
        {
            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, col, m_speed * Time.deltaTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 4, m_speed * Time.deltaTime);
            if (Mathf.Abs(mainCamera.orthographicSize - 4) < 0.01f)
            {
                break;
            }
            yield return null;
        }
        if(col == Color.red)
            yield return new WaitForSeconds(2);
        else
            yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
