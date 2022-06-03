using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public Animator winanim;
    public Animator gameOver;
    public Image gameStart;
    public Sprite[] gameStartSprites;
    public GameObject[] healthUI;
    private bool isGameStart;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    void Start()
    {
        PhotonNetwork.CurrentRoom.RemovedFromList = true;
        Destroy(GameObject.Find("BGmusic"));
        PhotonNetwork.AutomaticallySyncScene = false;
        int xposition = 0;
        GameObject player;
        switch (RoomManager.localPlayerPos)
        {
            case 1:
                xposition = -10;
                break;
            case 2:
                xposition = -4;
                break;
            case 3:
                xposition = 2;
                break;
            case 4:
                xposition = 8;
                break;
        }
        Vector3 startPos = new Vector3(xposition, 10f, 0);
        switch (RoomManager.roleIndex)
        {
            case 0:
                player = PhotonNetwork.Instantiate("FatMan", startPos, Quaternion.Euler(new Vector3(270, 0, 180)));
                break;
            case 1:
                player = PhotonNetwork.Instantiate("Warrior", startPos, Quaternion.Euler(new Vector3(270, 0, 180)));
                break;
            case 2:
                player = PhotonNetwork.Instantiate("Ninja", startPos, Quaternion.Euler(new Vector3(270, 0, 180)));
                break;
            case 3:
                player = PhotonNetwork.Instantiate("Guitarist", startPos, Quaternion.Euler(new Vector3(270, 0, 180)));
                break;
            default:
                player = null;
                break;
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            instance.healthUI[i].GetComponent<TMP_Text>().text = PhotonNetwork.PlayerList[i].NickName + ":";
            instance.healthUI[i].SetActive(true);
        }
        instance.healthUI[RoomManager.localPlayerPos - 1].GetComponent<TMP_Text>().color = Color.red;
        isGameStart = false;
        StartCoroutine(GameStart(player));
    }
    void Update()
    {
        if (isGameStart == false)
            return;
        if (GameObject.FindGameObjectsWithTag("Player").Length == 1)
        {
            instance.winanim.SetTrigger("win");
            Time.timeScale = 0f;
        }
    }
    public void GameOver()
    {
        instance.gameOver.SetTrigger("dead");
        isGameStart = false;
    }
    private IEnumerator GameStart(GameObject localPlayer)
    {
        instance.gameStart.gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            instance.gameStart.sprite = gameStartSprites[i];
            if (i == 3)
                instance.gameStart.transform.GetComponent<RectTransform>().localScale = new Vector3(6, 4, 1);
            yield return new WaitForSeconds(1);
        }
        Destroy(instance.gameStart.gameObject);
        localPlayer.GetComponent<PlayerCtrl>().isGameStart = true;
        isGameStart = true;
    }
}