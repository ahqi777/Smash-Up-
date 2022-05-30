using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class gamebutton : MonoBehaviourPun
{
    bool leftbool, rightbool;
    GameObject player;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        /*if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }*/
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (leftbool == true)
        {
            player.transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (rightbool == true)
        {
            player.transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
    }
    public void leftenter()
    {
        leftbool = true;
        //player.transform.Translate(speed * Time.deltaTime, 0, 0);
    }
    public void leftdown()
    {
        leftbool = false;
    }
    public void rightenter()
    {
        rightbool = true;
    }
    public void rightdown()
    {
        rightbool = false;
    }
}
