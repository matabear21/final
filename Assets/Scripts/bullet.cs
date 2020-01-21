﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if(ray.collider != null)
        {
            Debug.Log("땅");
            if(ray.collider.tag == "Enemy")
            {
                EnemyMove enemyMove = ray.collider.transform.GetComponent<EnemyMove>();
                enemyMove.OnDamaged();
            }
            else if (ray.collider.tag == "Player2")
            {
                Debug.Log("명중!");
                Player2Move player = ray.collider.transform.parent.GetComponent<Player2Move>();
                player.OnDamaged();
            }
            DestroyBullet();

        }
   
        if (transform.rotation.y == 0)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        { 
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }




}
