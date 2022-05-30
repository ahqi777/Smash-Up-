using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class launcher1 : MonoBehaviourPunCallbacks
{
    public GameObject roomUI,roomlistUI;
    public TMP_InputField roomname;
    bool start;
    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("disconnected");
    }
    public override void OnConnectedToMaster()
    {
        roomUI.SetActive(true);
        roomlistUI.SetActive(true);
    }
    public void roombutton()
    {
        if (roomname.text.Length < 2)
            return;
        if (roomname.text.Length > 9)
            return;
        roomUI.SetActive(false);
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomname.text, options,default);
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("leavelobby");
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.LoadLevel(4);
    }
    public void back()
    {
        roomUI.SetActive(false);
        roomlistUI.SetActive(false);
    }
}
