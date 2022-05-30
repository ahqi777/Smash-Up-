using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class RoomButton : MonoBehaviourPunCallbacks
{
    public AudioSource audioSource;
    public int count;
    public GameObject[] players;
    public TMP_Text roomName;
    public void JoinGameRoom()
    {
        PhotonNetwork.LeaveLobby();
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leave Room");
        }
        JoinRoom();
    }
    public void UpdatePlayerCount(RoomInfo roomInfo) 
    {
        for (int j = 0; j < players.Length; j++)
        {
            if ((j + 1) <= roomInfo.PlayerCount)
                players[j].SetActive(true);
            else
                players[j].SetActive(false);
        }
    }
    void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }
}
