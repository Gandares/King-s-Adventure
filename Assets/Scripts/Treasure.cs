using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public delegate void nowCanAttack();
    public event nowCanAttack OnNowCanAttack;
    public Sprite openChest;
    public GameObject text;
    private SpriteRenderer sr;
    public PlayerController pc;
    private bool openned = false;


    void Start(){
        sr = GetComponent<SpriteRenderer>();
        pc.OnOpenChest += open;
    }

    void OnDisable()
    {
        pc.OnOpenChest -= open;
    }

    void open(){
        if(!openned){
            sr.sprite = openChest;
            text.SetActive(true);
            OnNowCanAttack();
            openned = true;
        }
    }
}
