using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class Gamemanager : MonoBehaviourPunCallbacks
{
    public Animator winanim;
    GameObject AudioSource;
    //public GameObject[] playerlife;
    public int player1,xposition;
    //public string[] health;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        //Time.timeScale = 0f;
        Destroy(GameObject.Find("BGmusic"));
        
        //player1234 = choice1.player1234;
        /*if (player1234 == 1)
        {
            PhotonNetwork.Instantiate(health[0], new Vector3(-0.29f, 0.75f, 5.4f), Quaternion.identity);
        }
        if (player1234 == 2)
        {
            PhotonNetwork.Instantiate(health[1], new Vector3(-0.29f, 0.75f, 5.4f), Quaternion.identity);
        }
        else if (player1234 == 3)
        {
            PhotonNetwork.Instantiate(health[2], new Vector3(0.62f, 1.11f, 5.4f), Quaternion.identity);
        }
        else if (player1234 == 4)
        {
            PhotonNetwork.Instantiate(health[3], new Vector3(0.24f, 0.75f, 5.4f), Quaternion.identity);
        }*/
        PhotonNetwork.AutomaticallySyncScene = false;
        player1 = RoomManager.roleIndex;
        xposition = RoomManager.xposition;
        if (player1 == 1)
        {
            PhotonNetwork.Instantiate("fat", new Vector3(xposition, 10f, 0), Quaternion.identity);
        }
        else if (player1 == 2)
        {
            PhotonNetwork.Instantiate("soli", new Vector3(xposition, 10f, 0), Quaternion.identity);
        }
        else if (player1 == 3)
        {
            PhotonNetwork.Instantiate("nj", new Vector3(xposition, 10f, 0), Quaternion.identity);
        }
        else if (player1 == 4)
        {
            PhotonNetwork.Instantiate("gt", new Vector3(xposition, 10f, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            winanim.SetTrigger("win");
            Time.timeScale = 0f;
        }
    }
    /*public void startgame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (GameObject.Find("player1life").activeInHierarchy && GameObject.Find("player2life").activeInHierarchy)
            {
                PhotonNetwork.Instantiate("startgame", new Vector3(4.9f, 8.3f, -11.67f), Quaternion.identity);
            }
            else
            {

                Time.timeScale = 0f;
            }
        }
    }*/
    public void leavegame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel(1);
    }
    /*public override void OnMasterClientSwitched(Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel(1);
    }*/
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
    }
}