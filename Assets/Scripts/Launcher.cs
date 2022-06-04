using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class Launcher : MonoBehaviourPunCallbacks
{
    bool enterRoom;
    public GameObject nameUI,roomUI,roomlistUI;
    public TMP_InputField playerName, roomName;

    public GameObject roomlistbtn;
    public Transform roomlayout;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        else
            PhotonNetwork.JoinLobby();
    }
    void Update()
    {
        if (roomlistUI.activeSelf)
        {
            return;
        }
        if (PhotonNetwork.IsConnected)
        {
            if (enterRoom == false)
            {
                nameUI.SetActive(true);
            }
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomlayout.childCount; i++)
        {
            if (roomlayout.GetChild(i).gameObject.GetComponentInChildren<TMP_Text>().text == roomList[i].Name)
            {
                Destroy(roomlayout.GetChild(i).gameObject);

                if (roomList[i].PlayerCount == 0)
                {
                    roomList.RemoveAt(i);
                }
            }
        }
        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
                return;
            GameObject newroom = Instantiate(roomlistbtn, roomlayout.position, Quaternion.identity, roomlayout);
            newroom.GetComponent<RoomButton>().UpdatePlayerCount(room);
            newroom.GetComponentInChildren<TMP_Text>().text = room.Name;
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("disconnected");
    }
    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InRoom)
            PhotonNetwork.JoinLobby();
        Debug.Log("Enter Lobby");
    }
    /// <summary>
    /// 名字確定按鈕
    /// </summary>
    public void Nextbutton()
    {
        if (playerName.text.Length < 2)
            return;
        if (playerName.text.Length > 9)
            return;
        nameUI.SetActive(false);
        enterRoom = true;
        PhotonNetwork.NickName = playerName.text;
        roomUI.SetActive(true);
        roomlistUI.SetActive(true);
    }
    /// <summary>
    /// 創建房間按鈕
    /// </summary>
    public void Roombutton()
    {
        if (roomName.text.Length < 2)
            return;
        if (roomName.text.Length > 9)
            return;
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("Leave Room");
        }
        CreateRoom();
    }
    /// <summary>
    /// 創建房間
    /// </summary>
    void CreateRoom()
    {
        RoomOptions options = new RoomOptions { PublishUserId = true, MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, default);
        Debug.Log("Join Room");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LeaveLobby();
        Debug.Log("Leave Lobby");
        PhotonNetwork.LoadLevel("Room");
    }
}
