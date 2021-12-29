using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public delegate void openChest();
    public event openChest OnOpenChest;
    public delegate void axisButton(float s1, float s2);
    public event axisButton OnSetBgSpeed;
    private Vector3 spawnPoint;
    private float speed = 8f;
    private float jump = 900f;
    private bool canAttack = false;
    private float gravityScaleFalling = 4f;
    private float gravityScaleJumping = 2f;
    public LayerMask Ground;
    private float lastPositionX;
    private Rigidbody2D rb;
    private Animator a;
    private bool cantMove = false;
    private bool canJump = true;
    private bool isOnPlat;
    private Rigidbody2D platRB;
    public GameObject TextSign;
    public GameObject TextEntry;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        spawnPoint = new Vector3(-6.71f, 1.75f, 0f);
        lastPositionX = this.transform.position.x;
    }

    // Update is called once per frame
    void Update(){
        if(!cantMove){
            float horizontal = Input.GetAxisRaw("Horizontal");
            if(horizontal < -0.001)
                this.transform.rotation = Quaternion.Euler(0f,180f,0f);
            else if(horizontal > 0.001)
                this.transform.rotation = Quaternion.Euler(0f,0f,0f);

            a.SetFloat("Speed", Mathf.Abs(horizontal));

            transform.position += new Vector3(horizontal, 0, 0) * Time.deltaTime * speed;

            if(transform.position.x > lastPositionX + 0.1)
                OnSetBgSpeed(0.01f,0.04f);
            else if(transform.position.x < lastPositionX - 0.1)
                OnSetBgSpeed(-0.01f,-0.04f);
            else if(transform.position.x < lastPositionX + 0.1 && transform.position.x > lastPositionX - 0.1)
                OnSetBgSpeed(0f,0f);

            lastPositionX = transform.position.x;
        }

        if(Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 1.85f, Ground))
            canJump = true;
        else
            canJump = false;

        if(Input.GetMouseButtonDown(0) && canAttack){
            a.SetBool("Attack", true);
        }
    }

    void FixedUpdate()
    {
        if(!cantMove && canJump){
            if(Input.GetButton("Jump") && Mathf.Abs(rb.velocity.y) < 0.01f)
                rb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }

        if(isOnPlat){
            rb.velocity += platRB.velocity;
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
        
        if(other.gameObject.tag == "HouseEntry"){
            TextEntry.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {

        if(other.gameObject.tag == "HouseEntry"){
            if(Input.GetKeyDown(KeyCode.E)){
                this.transform.position = new Vector3(-70.3f,-3.35f,0f);
            }
        }

        if(other.gameObject.tag == "HouseExit"){
            if(Input.GetKeyDown(KeyCode.E)){
                this.transform.position = new Vector3(31.82f,29.7f,0f);
            }
        }

        if(other.gameObject.tag == "LevelExit"){
            if(Input.GetKeyDown(KeyCode.E)){
                SceneManager.LoadScene("Fin");
            }
        }

        if(other.gameObject.tag == "Treasure"){
            if(Input.GetKeyDown(KeyCode.E)){
                OnOpenChest();
                canAttack = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.gameObject.tag == "HouseSign"){
            TextSign.SetActive(false);
        }

        if(other.gameObject.tag == "HouseEntry"){
            TextEntry.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "plat"){
            platRB = other.gameObject.GetComponent<Rigidbody2D>();
            isOnPlat = true;
            Debug.Log("here");
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "plat"){
            isOnPlat = false;
            platRB = null;
            Debug.Log("There");
        }
    }

    IEnumerator w8Animation(){
        yield return new WaitForSeconds(1f);
        this.transform.position = spawnPoint;
        a.SetBool("Dead", false);
        cantMove = false;
    }
}
