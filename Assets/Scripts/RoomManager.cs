using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;


public class RoomManager : MonoBehaviourPunCallbacks
{
    public static int roleIndex;
    public static int localPlayerPos;

    public Sprite[] rolePrites;
    public Sprite[] Btnsprites;
    public GameObject[] players;
    public GameObject gameStart;
    public GameObject hint1, hint2;
    private Dictionary<int, string[]> playerRoomInfos;//Master儲存房間所有玩家資訊
    public string[] localPlayerInfo = { "", "", "", ""};//本地玩家資訊
    private int isReady;
    private bool isready;
    /// <summary>
    /// 初始化
    /// </summary>
    void Start()
    {
        playerRoomInfos = new Dictionary<int, string[]>();
        roleIndex = ChooseRole.roleindex;
        PhotonNetwork.AutomaticallySyncScene = true;
        localPlayerPos = PhotonNetwork.CurrentRoom.PlayerCount;//儲存第幾位玩家
        isReady = 0;

        localPlayerInfo[0] = roleIndex.ToString();//儲存玩家選擇
        localPlayerInfo[1] = PhotonNetwork.NickName;//儲存玩家名稱
        localPlayerInfo[2] = PhotonNetwork.PlayerList[localPlayerPos-1].UserId;//儲存玩家ID
        localPlayerInfo[3] = isReady.ToString();//儲存玩家準備狀態
        if (!PhotonNetwork.IsMasterClient)//如果是非Master，傳玩家資訊給Master
        {
            this.photonView.RPC("UpdateData", RpcTarget.MasterClient, localPlayerPos, localPlayerInfo);
        }
        else//是Master
        {
            if (playerRoomInfos.ContainsKey(localPlayerPos))//儲存玩家資訊在字典中 Key是玩家順序 Value是字串陣列，儲存玩家資訊
            {
                playerRoomInfos[localPlayerPos] = localPlayerInfo;
            }
            else
            {
                playerRoomInfos.Add(localPlayerPos, localPlayerInfo);
            }
            players[0].transform.GetChild(0).GetComponent<Image>().sprite = rolePrites[roleIndex];
            players[0].transform.GetChild(2).GetComponent<TMP_Text>().text = PhotonNetwork.NickName;
            players[0].SetActive(true);
        }
        Debug.Log("Enter Room");

    }
    private void Update()
    {
        for (int i = 0; i < 4; i++)//本地更新，控制名字以及按鈕
        {
            if (i == localPlayerPos - 1)//自己
            {
                players[i].transform.GetChild(1).GetComponent<Button>().enabled = true;
                players[i].transform.GetChild(2).GetComponent<TMP_Text>().color = Color.red;
            }
            else//其他玩家
            {
                players[i].transform.GetChild(1).GetComponent<Button>().enabled = false;
                players[i].transform.GetChild(2).GetComponent<TMP_Text>().color = Color.white;
            }
        }
    }
    /// <summary>
    /// 更新Master的資料，只有非Master可以呼叫
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="playerRoomInfo"></param>
    [PunRPC]
    private void UpdateData(int playerPos, string[] playerRoomInfo)
    {
        if (playerRoomInfos.ContainsKey(playerPos))
        {
            playerRoomInfos[playerPos] = playerRoomInfo;
        }
        else
        {
            playerRoomInfos.Add(playerPos, playerRoomInfo);
        }
        Debug.Log("寫入資料");
        this.photonView.RPC("UpdateUI", RpcTarget.All, playerRoomInfos);
    }
    [PunRPC]
    private void ReadyBtnOnClick(string playerID, int isReady)
    {
        for (int i = 0; i < playerRoomInfos.Count; i++)
        {
            if (playerID == playerRoomInfos[i + 1][2])
            {
                playerRoomInfos[i + 1][3] = isReady.ToString();
            }
        }
        this.photonView.RPC("UpdateUI", RpcTarget.All, playerRoomInfos);
    } 
    /// <summary>
    /// 更新UI資料，只有Master可以呼叫
    /// </summary>
    /// <param name="playerinfos"></param>
    [PunRPC]
    private void UpdateUI(Dictionary<int, string[]> playerinfos)
    {
        for (int i = 0; i < 4; i++)
        {
            if (playerinfos.ContainsKey(i + 1))
            {
                if (playerinfos[i + 1][2] == localPlayerInfo[2])//更新本地資料
                {
                    localPlayerPos = i + 1;
                    localPlayerInfo = playerinfos[i + 1];
                }

                string[] playerRoomInfo = playerinfos[i + 1];
                if (i != 0)//Master不用換按鈕狀態
                    players[i].transform.GetChild(1).GetComponent<Image>().sprite = Btnsprites[int.Parse(playerRoomInfo[3])];

                players[i].transform.GetChild(0).GetComponent<Image>().sprite = rolePrites[int.Parse(playerRoomInfo[0])];
                players[i].transform.GetChild(2).GetComponent<TMP_Text>().text = playerRoomInfo[1];
                players[i].SetActive(true);
            }
            else
            {
                players[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                players[i].transform.GetChild(2).GetComponent<TMP_Text>().text = null;
                players[i].SetActive(false);
            }
        }
    }
    /// <summary>
    /// 遊戲開始
    /// </summary>
    [PunRPC]
    private void GameStart()
    {
        StartCoroutine(GameStartAnim());
    }
    /// <summary>
    /// Master退出，解散房間
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ScenesManager.instance.LeaveRoom();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)//刪除離開玩家的資料，重新排序
        {
            for (int i = 2; i < playerRoomInfos.Count+1; i++)
            {
                if (playerRoomInfos.ContainsKey(i))
                {
                    if (otherPlayer.UserId == playerRoomInfos[i][2])
                    {
                        playerRoomInfos.Remove(i);
                        Debug.Log("刪除資料");
                        if (!playerRoomInfos.ContainsKey(3) && playerRoomInfos.ContainsKey(4))
                        {
                            playerRoomInfos[i] = playerRoomInfos[i + 1];
                            playerRoomInfos.Remove(i + 1);
                        }
                        else if (playerRoomInfos.ContainsKey(3) && !playerRoomInfos.ContainsKey(4))
                        {
                            if (i != 4)
                            {
                                playerRoomInfos[i] = playerRoomInfos[i + 1];
                                playerRoomInfos.Remove(i + 1);
                            }
                        }
                        else if (playerRoomInfos.ContainsKey(3) && playerRoomInfos.ContainsKey(4))
                        {
                            playerRoomInfos[i] = playerRoomInfos[i + 1];
                            playerRoomInfos[i + 1] = playerRoomInfos[i + 2];
                            playerRoomInfos.Remove(i + 2);
                        }
                    } 
                }
            }
            this.photonView.RPC("UpdateUI", RpcTarget.All, playerRoomInfos);//再次更新UI
        }
    }
    /// <summary>
    /// 準備按鈕
    /// </summary>
    public void GetReady_Btn()
    {
        isready = !isready;//反轉準備狀態
        if (isready == true)
            isReady = 1;
        else
            isReady = 0;
        this.photonView.RPC("ReadyBtnOnClick", RpcTarget.MasterClient, localPlayerInfo[2], isReady);//傳給Master更新資料
    }
    /// <summary>
    /// 開始按鈕
    /// </summary>
    public void GetStart_Btn()
    {
        int temp = 0; 
        for (int i = 0; i < playerRoomInfos.Count; i++)
        {
            if (playerRoomInfos[i + 1][3] == 1.ToString())
                temp++;
        }
        if (temp == PhotonNetwork.CurrentRoom.PlayerCount - 1 && temp != 0)//判斷是否所有人都準備
        {
            this.photonView.RPC("GameStart", RpcTarget.All);
        }
        else
        {
            Hint();//提示房主有人沒準備
        }
    }
    /// <summary>
    ///房間提示
    /// </summary>
    private void Hint()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            hint1.SetActive(true);
            hint2.SetActive(false);
        }
        else
        {
            hint1.SetActive(false);
            hint2.SetActive(true);
        }
    }
    private IEnumerator GameStartAnim()
    {
        gameStart.SetActive(true);
        while (gameStart.GetComponent<RectTransform>().anchoredPosition3D.x < 0)
        {
            gameStart.GetComponent<RectTransform>().anchoredPosition3D += new Vector3(5f, 0, 0);
            gameStart.GetComponent<RectTransform>().localScale += new Vector3(0.02f, 0.02f, 0.02f);
            yield return null;
        }
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Game");
    }
}
