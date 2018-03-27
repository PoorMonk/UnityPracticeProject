# UnityPracticeProject

单例模式：

	static public GameManager _instance = null;
	void Awake () {       
		if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject)；
    }

为player和enemy抽象一个基类来实现移动

	public abstract class MoveObject : MonoBehaviour {

		//start为虚函数，以便在子类中能重写
		protected virtual void Start () 
		{
	        boxCollier = GetComponent<BoxCollider2D>();
	        rb2D = GetComponent<Rigidbody2D>();
	        inverseMoveTime = 1f / moveTime;
		}
		
		protected IEnumerator SmoothMovement(Vector3 end)
	    {
	        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
	
	        while(sqrRemainingDistance > float.Epsilon)
	        {
	            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
	            rb2D.MovePosition(newPosition);
	            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
	            yield return null;
	        }
	    }
	
	    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
	    {
	        Vector2 start = transform.position;
	        Vector2 end = start + new Vector2(xDir, yDir);
	        boxCollier.enabled = false;
	        hit = Physics2D.Linecast(start, end, blockingLayer); //进行碰撞检测
	        boxCollier.enabled = true;
	        if (hit.transform == null)
	        {
	            StartCoroutine(SmoothMovement(end));
	            return true;
	        }
	        return false;
	    }
	
		//泛型virtual函数在基类中必须有实现体
	    protected virtual void AttemptMove<T>(int xDir, int yDir)
	        where T : Component
	    {
	        RaycastHit2D hit;
	        bool canMove = Move(xDir, yDir, out hit);
	        if (hit.transform == null)
	            return;
	        T hitComponent = hit.transform.GetComponent<T>();
	        if (!canMove && hitComponent != null)
	            OnCantMove(hitComponent);
	    }
	
		//泛型abstract函数只能在子类中实现
	    protected abstract void OnCantMove<T>(T component)
	        where T : Component;

场景加载

	if(collision.tag == "Exit")
    {
        Invoke("Restart", restartLevelDelay); //调用重启函数
        enabled = false;
    }

	private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);  //触发场景加载事件      
    }
	
	GameManager类：
	void OnEnable()
    {       
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {     
        InitGame();
        level++;
    }

地图创建

	private List<Vector3> gridPositions = new List<Vector3>();
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

	Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count - 1); //最后一个格子用来放置过关图标
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); //把随机的格子移除，保证下次随机位置不会重复
        return randomPosition;
    }