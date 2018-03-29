using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

    private Transform StartPoint;
    private Transform DiskPoint;

    [HideInInspector]public bool IsReachStartPoint = false;
    [HideInInspector]public bool IsGotoCircle = false;
    public float FlySpeed = 5f;
    private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
        StartPoint = GameObject.Find("StartPoint").transform;
        DiskPoint = GameObject.Find("Disk").transform;
        targetPosition = DiskPoint.position;
        targetPosition.y -= 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsGotoCircle)
        {
            if (!IsReachStartPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPoint.position, FlySpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, StartPoint.position) < 0.05f)
                {
                    IsReachStartPoint = true;
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, FlySpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {             
                transform.position = targetPosition;
                transform.SetParent(DiskPoint);
                IsGotoCircle = false;
            }
        }
	}

    public void fly()
    {
        IsGotoCircle = true;
        IsReachStartPoint = true;
    }
}
