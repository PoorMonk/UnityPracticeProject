using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MoveObject {

    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int wallDamage = 1;
    public Text foodText;

    private Animator animator;
    private int food;

    public AudioClip moveSound1;                //1 of 2 Audio clips to play when player moves.
    public AudioClip moveSound2;                //2 of 2 Audio clips to play when player moves.
    public AudioClip eatSound1;                 //1 of 2 Audio clips to play when player collects a food object.
    public AudioClip eatSound2;                 //2 of 2 Audio clips to play when player collects a food object.
    public AudioClip drinkSound1;               //1 of 2 Audio clips to play when player collects a soda object.
    public AudioClip drinkSound2;               //2 of 2 Audio clips to play when player collects a soda object.
    public AudioClip gameOverSound;

    // Use this for initialization
    protected override void Start () {
        
        animator = GetComponent<Animator>();
        food = GameController._instance.playerFoodPoints;
        foodText.text = "Food : " + food;
        base.Start();
        //Debug.Log("Start");
	}

    private void OnDisable()
    {
        GameController._instance.playerFoodPoints = food;
        Debug.Log("OnDisable");
    }

    // Update is called once per frame
    void Update () {
        if (!GameController._instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if(horizontal !=0 || vertical != 0 )
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food : " + food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomMusic(moveSound1, moveSound2);
        }
        CheckIfGameOver();
        GameController._instance.playersTurn = false;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameController._instance.GameOver();
            SoundManager.instance.RandomMusic(gameOverSound);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playChop");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Exit")
        {
            Debug.Log("EnterExit");
            collision.gameObject.SetActive(false);
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Food")
        {
            //Debug.Log("EnterFood");
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food : " + food;
            SoundManager.instance.RandomMusic(eatSound1, eatSound2);
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food : " + food;
            SoundManager.instance.RandomMusic(drinkSound1, drinkSound2);
            collision.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //Debug.Log("Restart");
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playHit");
        food -= loss;
        foodText.text = "-" + loss + " Food : " + food;
        CheckIfGameOver();
    }
}
