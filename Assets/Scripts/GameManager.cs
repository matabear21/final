using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 점수와 스테이지를 관리하는 역할

    public int stageIndex;
    public PlayerMove player;
    public Player2Move player2;
    public float retime;

    public GameObject[] flags;

    public GameObject[] Stages;
    public Image[] stageinfo;

    public GameObject[] trap;

    public GameObject start_menu;
    public Button start_btn;
    public Button manual_btn;
    public Button manual_close_btn;
    public Image manual;

    //setting button
    public Button settingBtn;
    public Button resumeBtn;
    public Button restartBtn;
    public Button exitBtn;


    public void Start()
    {
        start_btn.onClick.AddListener(startClick);
        manual_btn.onClick.AddListener(manualClick);
        manual_close_btn.onClick.AddListener(manualCloseClick);
        settingBtn.onClick.AddListener(settingOn);
        resumeBtn.onClick.AddListener(resumeClick);
        restartBtn.onClick.AddListener(restartClick);
        exitBtn.onClick.AddListener(exitClick);

        Time.timeScale = 0;
        manual.enabled = false;
        manual_close_btn.image.enabled = false;
        settingBtn.enabled = false;
        settingBtn.image.enabled = false;

        for (int i = 0; i < stageinfo.Length; i++)
        {
            stageinfo[i].enabled = false;
        }

        settingOff();

    }
    public void Update()
    {
        for (int i = 0; i < trap.Length; i++)
        {
            trapSet(trap[i]);
        }
    }

    public void trapSet(GameObject trap)
    {
        if (Mathf.Abs(trap.transform.position.x - player.transform.position.x) < 2 || Mathf.Abs(trap.transform.position.x - player2.transform.position.x) < 2)
            trap.SetActive(true);
        else
        {
            trap.SetActive(false);
        }
    }

    void startClick()
    {

        start_menu.SetActive(false);
        Time.timeScale = 1;
        settingBtn.enabled = true;
        settingBtn.image.enabled = true;
        stageIndex = 2;
        for (int i = 0; i < stageIndex; i++)
        {
            Stages[i].SetActive(false);
            stageinfo[i].enabled = true;
            stageinfo[i].color = new Color(255, 0, 0);
        }
        Stages[stageIndex].SetActive(true);
        stageinfo[stageIndex].enabled = true;
        stageinfo[stageIndex].color = new Color(255, 255, 255);
        for (int i = stageIndex + 1; i < stageinfo.Length; i++)
        {
            Stages[i].SetActive(false);
            stageinfo[i].enabled = true;
            stageinfo[i].color = new Color(0, 0, 255);
        }
    }
    void manualClick() { manual.enabled = true; manual_close_btn.image.enabled = true; manual_close_btn.enabled = true; }
    void manualCloseClick() { manual.enabled = false; manual_close_btn.image.enabled = false; manual_close_btn.enabled = false; }

    void settingOn()
    {
        Time.timeScale = 0;
        resumeBtn.image.enabled = true;
        resumeBtn.enabled = true;
        restartBtn.image.enabled = true;
        restartBtn.enabled = true;
        exitBtn.image.enabled = true;
        exitBtn.enabled = true;
    }
    void settingOff()
    {
        resumeBtn.image.enabled = false;
        resumeBtn.enabled = false;
        restartBtn.image.enabled = false;
        restartBtn.enabled = false;
        exitBtn.image.enabled = false;
        exitBtn.enabled = false;
    }

    void resumeClick() { Time.timeScale = 1; settingOff(); }
    void restartClick()
    {
        stageIndex = 2;
        flags[stageIndex * 2].SetActive(true);
        flags[stageIndex * 2 + 1].SetActive(true);

        for (int i = 0; i < stageIndex; i++)
        {
            Stages[i].SetActive(false);
            stageinfo[i].enabled = true;
            stageinfo[i].color = new Color(255, 0, 0);
        }
        Stages[stageIndex].SetActive(true);
        stageinfo[stageIndex].enabled = true;
        stageinfo[stageIndex].color = new Color(255, 255, 255);
        for (int i = stageIndex + 1; i < stageinfo.Length; i++)
        {
            Stages[i].SetActive(false);
            stageinfo[i].enabled = true;
            stageinfo[i].color = new Color(0, 0, 255);
        }
        settingOff();
        Time.timeScale = 1;

        player.transform.position = new Vector2(-4.5f, -4);
        player2.transform.position = new Vector2(5, -4);

    }
    void exitClick()
    {
        Time.timeScale = 0;
        settingOff();
        restartClick();
        start_menu.SetActive(true);
        settingBtn.enabled = false;
        settingBtn.image.enabled = false;
        for (int i = 0; i < stageinfo.Length; i++)
        {
            stageinfo[i].enabled = false;
        }
    }


    public void NextStage()
    {
        stageinfo[stageIndex].color = new Color(255, 0, 0);
        if (stageIndex < Stages.Length - 1)
        {
            flags[stageIndex * 2].SetActive(false);
            flags[stageIndex * 2 + 1].SetActive(false);
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            flags[stageIndex * 2].SetActive(true);
            flags[stageIndex * 2 + 1].SetActive(true);
            stageinfo[stageIndex].color = new Color(255, 255, 255);
            // 맵의 길이와 중심, flag의 위치를 모두 통일 시킴.
            player.transform.position = new Vector3(-26, -3, -1);
            PlayerReposition(2);

        }
        else  //game clear
        {
            settingOn();
            resumeBtn.image.enabled = false;
            resumeBtn.enabled = false;
            Debug.Log("게임 클리어!");

        }
    }

    public void PrevStage()
    {
        stageinfo[stageIndex].color = new Color(0, 0, 255);
        if (stageIndex > 0)
        {
            flags[stageIndex * 2].SetActive(false);
            flags[stageIndex * 2 + 1].SetActive(false);
            Stages[stageIndex].SetActive(false);
            stageIndex--;
            Stages[stageIndex].SetActive(true);
            flags[stageIndex * 2].SetActive(true);
            flags[stageIndex * 2 + 1].SetActive(true);
            stageinfo[stageIndex].color = new Color(255, 255, 255);
            player2.transform.position = new Vector3(27, -2, -1);
            PlayerReposition(1);

        }
        else  //game clear
        {
            settingOn();
            resumeBtn.image.enabled = false;
            resumeBtn.enabled = false;
            Debug.Log("게임 클리어!");


        }

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //원위치로
            player.OnDamaged();
        }
        if (collision.gameObject.tag == "Player2")
        {

            player2.OnDamaged();
        }
    }

    public void PlayerReposition(int i)
    {
        retime = 0.5f;
        while (retime <= 0) { retime -= Time.deltaTime; }

        {

            if (!player.isLive && !player2.isLive)
            {
                player.transform.position = new Vector3(player.transform.position.x, 1, -1);
                player2.transform.position = new Vector3(player2.transform.position.x, 1, -1);
                return;
            }
            if (i == 1)
            {
                player.transform.position = player2.transform.position + new Vector3(-8, 10, -1);
                player.VelocityZero();
            }
            else if (i == 2)
            {
                player2.transform.position = player.transform.position + new Vector3(8, 10, -1);
                player2.VelocityZero();
            }
        }



    }
    void StartPosition()
    {
        player.transform.position = new Vector3(-5, 0, -1);
        player.VelocityZero();

        player2.transform.position = new Vector3(5, 0, -1);
        player2.VelocityZero();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
