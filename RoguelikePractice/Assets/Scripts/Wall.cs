using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite;
    public int hp = 3;
    private SpriteRenderer _SpriteRenderer;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    // Use this for initialization
    void Awake () {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public void DamageWall(int loss)
    {
        _SpriteRenderer.sprite = dmgSprite;
        hp -= loss;
        SoundManager.instance.RandomMusic(chopSound1, chopSound2);
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
