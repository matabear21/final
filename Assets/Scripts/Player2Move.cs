using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Move : MonoBehaviour
{
    public float maxSpeed = 4;
    public float jumpPower;
    public GameManager gameManager;
    public PlayerMove player1;
    public GameObject bullet2;
    public Transform pos;
    public float cooltime;
    private float curtime;

    public float livetime;
    public bool isLive = true;
    public int razer_sword = -1;
    public GameObject razer_collider;
    public GameObject sword_collider;
    public GameObject crouchcollider;
    public GameObject sword_sword_collider;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    AudioSource audioSource;
    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("is_razer_idle", true);
        razer_sword = Random.Range(-1, 1);
        if (razer_sword == 0) anim.SetBool("is_razer_idle", false);
        Debug.Log(razer_sword);
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        if (razer_sword == -1)
        {
            razer_collider.SetActive(true);
            sword_collider.SetActive(false);
            crouchcollider.SetActive(false);
            sword_sword_collider.SetActive(false);
        }
        else if (razer_sword == 0)
        {
            razer_collider.SetActive(false);
            sword_collider.SetActive(true);
            crouchcollider.SetActive(false);
            sword_sword_collider.SetActive(false);
        }
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }


    private void Update() //1초에 60번 돔. 단발적인 키 입력
    {  //버튼 떼기
        if (!isLive) return;
        livetime -= Time.deltaTime;
        if (livetime < 0) rigid.gravityScale = 1;

        if (razer_sword == -1)
        {
            //Jump
            if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("is_razer_jump") && !anim.GetBool("is_razer_crouch"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("is_razer_jump", true);
                PlaySound("JUMP");
            }

            //양옆 이동
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (anim.GetBool("is_razer_crouch"))
                    rigid.velocity = new Vector2(0, 0);
                if (Input.GetKeyUp(KeyCode.RightArrow))
                    maxSpeed = 4;
                rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
            }

            //Direction Sprite getbutton down  에서 get down으로 바꾸면 문워크가 사라짐.
            if (Input.GetKeyDown(KeyCode.RightArrow))
                maxSpeed = maxSpeed * 6 / 5;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (anim.GetBool("is_razer_crouch"))
                        rigid.velocity = new Vector2(0, 0);
                    //spriteRenderer.flipX = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);


                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (anim.GetBool("is_razer_crouch"))
                        rigid.velocity = new Vector2(0, 0);
                    //spriteRenderer.flipX = false;
                    transform.eulerAngles = new Vector3(0, 180, 0);

                }
            }
            //Animation
            if (Mathf.Abs(rigid.velocity.x) < 0.3)
                anim.SetBool("is_razer_walk", false);
            else
                anim.SetBool("is_razer_walk", true);

            if (Input.GetKeyDown(KeyCode.DownArrow) && !anim.GetBool("is_razer_jump") && !anim.GetBool("is_razer_attack"))
            {
                anim.SetBool("is_razer_crouch", true);


                razer_collider.SetActive(false);
                crouchcollider.SetActive(true);


            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.SetBool("is_razer_crouch", false);

                razer_collider.SetActive(true);
                crouchcollider.SetActive(false);


            }



            if (Input.GetKey(KeyCode.RightShift) && !anim.GetBool("is_razer_crouch") && player1.isLive)
            {

                if (curtime <= 0&&player1.livetime<=0)
                {
                    if (anim.GetBool("is_razer_jump"))
                        anim.SetBool("is_razer_jump_attack", true);
                    else
                    {
                        anim.SetBool("is_razer_attack", true);
                    }
                    Instantiate(bullet2, pos.position,transform.rotation);
                    curtime = cooltime;

                }
                curtime -= Time.deltaTime;
                PlaySound("ATTACK");
            }
            player1.livetime -= Time.deltaTime;


            if (Input.GetKeyUp(KeyCode.RightShift))
            {

                anim.SetBool("is_razer_attack", false);
                anim.SetBool("is_razer_jump_attack", false);
            }
            curtime -= Time.deltaTime;
        }
        else if (razer_sword == 0)
        {
            //Jump
            if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("is_sword_jump") && !anim.GetBool("is_razer_crouch"))
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("is_sword_jump", true);
                PlaySound("JUMP");
            }

            //양옆 이동
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (anim.GetBool("is_razer_crouch"))
                    rigid.velocity = new Vector2(0, 0);
                if (Input.GetKeyUp(KeyCode.RightArrow))
                    maxSpeed = 4;
                rigid.velocity = new Vector2(0.5f * rigid.velocity.normalized.x, rigid.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
                maxSpeed = maxSpeed * 6 / 5;

            //Direction Sprite getbutton down  에서 get down으로 바꾸면 문워크가 사라짐.
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (anim.GetBool("is_razer_crouch"))
                        rigid.velocity = new Vector2(0, 0);
                    //spriteRenderer.flipX = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);


                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (anim.GetBool("is_razer_crouch"))
                        rigid.velocity = new Vector2(0, 0);
                    //spriteRenderer.flipX = false;
                    transform.eulerAngles = new Vector3(0, 180, 0);

                }
            }
            //Animation
            if (Mathf.Abs(rigid.velocity.x) < 0.3)
                anim.SetBool("is_sword_walk", false);
            else
                anim.SetBool("is_sword_walk", true);

            if (Input.GetKeyDown(KeyCode.DownArrow) && !anim.GetBool("is_sword_jump") && !anim.GetBool("is_razer_attack"))
            {
                anim.SetBool("is_razer_crouch", true);


                sword_collider.SetActive(false);
                crouchcollider.SetActive(true);


            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.SetBool("is_razer_crouch", false);

                sword_collider.SetActive(true);
                crouchcollider.SetActive(false);


            }



            if (Input.GetKey(KeyCode.RightShift) && !anim.GetBool("is_razer_crouch"))
            {

                    if (anim.GetBool("is_sword_jump"))
                    {
                        anim.SetBool("is_sword_jump_attack", true);
                        sword_sword_collider.SetActive(true);
                    }
                    else
                    {
                        anim.SetBool("is_sword_attack", true);
                        sword_sword_collider.SetActive(true);
                    }
                PlaySound("ATTACK");
                  
            }


            if (Input.GetKeyUp(KeyCode.RightShift))
            {

                anim.SetBool("is_sword_attack", false);
                anim.SetBool("is_sword_jump_attack", false);
                sword_sword_collider.SetActive(false);
            }
            curtime -= Time.deltaTime;
        }



    }

    // Update is called once per frame(지속적인 키 입력)
    void FixedUpdate()
    {
        //addforce는 1초에 50번 힘을 줌.
        //Move by control
        //move speed
        float h = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            h = -1;
        else if (Input.GetKey(KeyCode.RightArrow))
            h = 1;

        //float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //maxspeed
        if (rigid.velocity.x > maxSpeed) //Right max speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) //left max speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y <= 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(1, 0, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.8f)
                {
                    anim.SetBool("is_razer_jump", false);
                    anim.SetBool("is_razer_jump_attack", false);
                    anim.SetBool("is_razer_attack", false);

                    anim.SetBool("is_sword_jump", false);
                    anim.SetBool("is_sword_jump_attack", false);
                    //anim.SetBool("is_sword_attack", false);
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 이부분 수정
        if (collision.gameObject.tag == "Enemy")
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            if (anim.GetBool("is_sword_attack") || anim.GetBool("is_sword_jump_attack"))
                OnAttack(collision.transform);
            else
                OnDamaged();
        }
        else if (collision.gameObject.tag == "Player")
        {

            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                PlayerMove player = collision.transform.GetComponent<PlayerMove>();
                player.OnDamaged();
            }
            if (anim.GetBool("is_sword_attack") || anim.GetBool("is_jump_sword_attack"))
            {

                Debug.Log("칼빵");
                PlayerMove player = collision.transform.GetComponent<PlayerMove>();
                player.OnDamaged();
            }
        }
        else if (collision.gameObject.tag == "trap")
            OnDamaged();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish_1_1")
        {
            //Next stage
            gameManager.PrevStage();
            PlaySound("FINISH");
        }
        else if (collision.gameObject.tag == "Finish_2_1")
        {
            //Next stage
            gameManager.PrevStage();
            PlaySound("FINISH");
        }
        else if (collision.gameObject.tag == "Finish_3_1")
        {
            //Next stage
            gameManager.PrevStage();
            PlaySound("FINISH");
        }

    }


    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void OnAttack(Transform enemy)
    {
        //Point
        //Reaction Force
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();

    }

    public void OnDamaged()
    {
        PlaySound("DAMAGED");
        //투명도에 0.4 넣음
        spriteRenderer.color = new Color(0.4156f, 0.6941f, 0.9725f, 0.4f);
        spriteRenderer.flipY = true;

        //애니 변수들 razer_idle로 맞추기
        anim.SetBool("is_razer_walk", false);
        anim.SetBool("is_razer_attack", false);
        anim.SetBool("is_razer_crouch", false);
        anim.SetBool("is_razer_jump", false);
        anim.SetBool("is_razer_jump_attack", false);
        anim.SetBool("is_sword_walk", false);
        anim.SetBool("is_sword_attack", false);
        anim.SetBool("is_sword_jump", false);
        anim.SetBool("is_sword_jump_attack", false);
        anim.SetBool("is_razer_idle", false);

        razer_collider.SetActive(false);
        sword_collider.SetActive(false);
        crouchcollider.SetActive(false);
        sword_sword_collider.SetActive(false);

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        isLive = false;

        Invoke("OnLive", 2);
    }
    public void OnLive()
    {
        
        spriteRenderer.color = new Color(0.4156f,0.6941f, 0.9725f, 1);
        spriteRenderer.flipY = false;
        gameManager.PlayerReposition(2);
        isLive = true;
        anim.SetBool("is_razer_idle", true);
        razer_sword = Random.Range(-1, 1);
        Debug.Log("확인하기" + razer_sword);
        if (razer_sword == 0) anim.SetBool("is_razer_idle", false);
   
        if (razer_sword == -1)
        {
          
            razer_collider.SetActive(true);
            sword_collider.SetActive(false);
            crouchcollider.SetActive(false);
            sword_sword_collider.SetActive(false);
        }
        else if (razer_sword == 0)
            
        {
            razer_collider.SetActive(false);
            sword_collider.SetActive(true);
            crouchcollider.SetActive(false);
            sword_sword_collider.SetActive(false);
        }
        maxSpeed = 4;
        rigid.gravityScale = 4;
        livetime = 1;
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
