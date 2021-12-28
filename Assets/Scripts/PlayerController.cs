using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector3 spawnPoint;
    private float speed = 8f;
    private float jump = 900f;
    private float gravityScaleFalling = 4f;
    private float gravityScaleJumping = 2f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator a;
    private bool cantMove = false;
    public GameObject TextSign;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        spawnPoint = new Vector3(-6.71f, 1.75f, 0f);
    }

    // Update is called once per frame
    void Update(){
        if(!cantMove){
            float horizontal = Input.GetAxisRaw("Horizontal");
            if(horizontal < -0.001)
                sr.flipX = true;
            else if(horizontal > 0.001)
                sr.flipX = false;

            a.SetFloat("Speed", Mathf.Abs(horizontal));

            transform.position += new Vector3(horizontal, 0, 0) * Time.deltaTime * speed;
        }
    }

    void FixedUpdate()
    {
        if(!cantMove){
            if(Input.GetButton("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
                rb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }

            a.SetFloat("High", rb.velocity.y);

            if(rb.velocity.y > 0)
                rb.gravityScale = gravityScaleJumping;
            if(rb.velocity.y <= 0)
                rb.gravityScale = gravityScaleFalling;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.tag == "Dead"){
            a.SetBool("Dead", true);
            cantMove = true;
            StartCoroutine(w8Animation());
        }

        if(other.gameObject.tag == "CheckPoint"){
            spawnPoint = other.gameObject.GetComponent<Transform>().position;
            Debug.Log(spawnPoint[0] + ", " + spawnPoint[1] + ", " + spawnPoint[2]);
        }

        if(other.gameObject.tag == "HouseSign"){
            TextSign.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.gameObject.tag == "HouseSign"){
            TextSign.SetActive(false);
        }
    }

    IEnumerator w8Animation(){
        yield return new WaitForSeconds(2f);
        this.transform.position = spawnPoint;
        a.SetBool("Dead", false);
        cantMove = false;
    }
}
