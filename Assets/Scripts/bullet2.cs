using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet2 : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 0.8f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                EnemyMove enemyMove = ray.collider.transform.GetComponent<EnemyMove>();
                enemyMove.OnDamaged();
            }
            else if (ray.collider.tag == "Player")
            {
                Debug.Log("명중!");
                PlayerMove player = ray.collider.transform.parent.GetComponent<PlayerMove>();
                player.OnDamaged();
            }
            DestroyBullet();
        }

        if (transform.rotation.y == 0)
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right  * speed * Time.deltaTime);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }




}
