using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float timeMoving;
    public float speed;
    public bool up;
    private float otherWay = 0;
    private bool done = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(otherWay%2 == 0){
            if(up)
                this.transform.position += new Vector3(0, 1, 0) * Time.deltaTime * speed;
            else
                this.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }
        else{
            if(up)
                this.transform.position += new Vector3(0, -1, 0) * Time.deltaTime * speed;
            else
                this.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
        }
        if(!done){
            done = true;
            StartCoroutine(changeDirection());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        other.gameObject.transform.SetParent(gameObject.transform, true);
    }

    private void OnCollisionExit2D(Collision2D other) {
        other.gameObject.transform.parent = null;
    }

    IEnumerator changeDirection(){
        yield return new WaitForSeconds(timeMoving);
        otherWay++;
        done = false;
    }
}
