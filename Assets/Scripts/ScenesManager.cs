using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviourPunCallbacks
{
    public static ScenesManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    public void GoLogin()
    {
        SceneManager.LoadScene("Login");
    }
    public void GoTeach()
    {
        SceneManager.LoadScene("Teach");
    }
    public void GoChoice()
    {
        SceneManager.LoadScene("Choice");
    }
    public void GoLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Choice");
    }
    public void GoRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }
    public void GoGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
        PhotonNetwork.JoinLobby();
    }
    public void GameQuit()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
}
