using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinHead : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Head"))
        {
            GameManager.instance.GameOver(Color.red);
            GameManager.instance.InitResource();
            Debug.Log("Over");
        }
    }
}
