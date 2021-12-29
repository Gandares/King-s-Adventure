using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{


    public Treasure t;
    private bool canAttack = false;
    public ParticleSystem p;

    void Start(){
        t.OnNowCanAttack += setCanAttack;
    }

    void OnDisable()
    {
        t.OnNowCanAttack -= setCanAttack;
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Attack"){
            if(Input.GetMouseButtonDown(0) && canAttack){
                StartCoroutine(DelayDestroy());
            }
        }
    }

    void setCanAttack(){
        canAttack = true;
    }

    IEnumerator DelayDestroy(){
        yield return new WaitForSeconds(0.5f);
        Instantiate(p, this.transform.position + new Vector3(0f,0f,-1f), Quaternion.identity);
        Destroy(this.gameObject);
    }
}
