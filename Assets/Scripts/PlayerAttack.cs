using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet1;
    public Transform pos;
    public float cooltime;
    private float curtime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curtime <= 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Instantiate(bullet1, pos.position, transform.rotation);
                curtime = cooltime;
            }
        }
        curtime -= Time.deltaTime;
    }
}
