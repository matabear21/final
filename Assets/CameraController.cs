using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerMove player1;
    public Player2Move player2;
    // Start is called before the first frame update
    void Start()
    {
        
        //Camera.main.orthographicSize = Mathf.Abs(player1.transform.position.x - player2.transform.position.x) * 0.5f;
        transform.position = (player1.transform.position + player2.transform.position) / 2+ new Vector3(0,0,-10);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float c_size = Mathf.Abs(player1.transform.position.x - player2.transform.position.x) * 0.6f;
        if (c_size > 25)
        {
            Camera.main.orthographicSize = 25;
            return;
        }
        if (c_size >= 5)
            Camera.main.orthographicSize = c_size;
        else
            Camera.main.orthographicSize = 5.0f;

        if (player1.isLive && player2.isLive)
        {
            if(player1.transform.position.y > 7) transform.position = new Vector3(player1.transform.position.x + player2.transform.position.x, player2.transform.position.y * 2, 0) / 2 + new Vector3(0, 0, -10);
            else if(player2.transform.position.y > 7) transform.position = new Vector3(player1.transform.position.x + player2.transform.position.x, player1.transform.position.y * 2, 0) / 2 + new Vector3(0, 0, -10);
            else transform.position = new Vector3(player1.transform.position.x + player2.transform.position.x, player1.transform.position.y + player2.transform.position.y, 0) / 2 + new Vector3(0, 0, -10);
        }
        else if (player1.isLive)
            transform.position = new Vector3(player1.transform.position.x + player2.transform.position.x, player1.transform.position.y*2 , 0) / 2 + new Vector3(0, 0, -10);
        else if(player2.isLive)
            transform.position = new Vector3(player1.transform.position.x + player2.transform.position.x, player2.transform.position.y*2, 0) / 2 + new Vector3(0, 0, -10);
        else
            transform.position = new Vector3(player1.transform.position.x + player2.transform.position.x, player1.transform.position.y + player2.transform.position.y, 0) / 2 + new Vector3(0, 0, -10);


    }
}
