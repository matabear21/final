using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplatform : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove=1;
    public float turntime;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        turn();
    }


    void turn()
    {
        nextMove *= -1;
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        Invoke("turn", 1.5f);
    }
}
