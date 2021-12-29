using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private float bgSpeed1 = 0f;
    public Renderer bgRend1;
    private float bgSpeed2 = 0f;
    public Renderer bgRend2;
    public PlayerController pc;
    void Start()
    {
        pc.OnSetBgSpeed += setSpeed;
    }

    void OnDisable()
    {
        pc.OnSetBgSpeed -= setSpeed;
    }

    void Update()
    {
        bgRend1.material.mainTextureOffset += new Vector2(bgSpeed1 * Time.deltaTime, 0f);
        bgRend2.material.mainTextureOffset += new Vector2(bgSpeed2 * Time.deltaTime, 0f);
    }

    void setSpeed(float s1, float s2){
        bgSpeed1 = s1;
        bgSpeed2 = s2;
    }
}
