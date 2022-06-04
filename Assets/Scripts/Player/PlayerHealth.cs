using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
public class PlayerHealth : MonoBehaviourPunCallbacks
{
    PlayerCtrl playerCtrl;
    Animator anim;
    public int healthcount;
    public bool hurtopen;
    public bool inbubble;
    public Collider ishurt;
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        anim = GetComponent<Animator>();
        healthcount = 100;
        hurtopen = true;
        inbubble = false;
    }
    /// <summary>
    /// 更新血量UI
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="health"></param>
    [PunRPC]
    private void UpdateHealth(int playerPos, int health, Player owner)
    {
        GameObject healthUI = GameObject.Find("P" + (playerPos).ToString() + "Name").transform.GetChild(1).gameObject;
        healthUI.GetComponent<TMP_Text>().text = health.ToString();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().Owner == owner)
            {
                players[i].GetComponent<PlayerHealth>().healthcount = health;
            }
        }
        CheckDead();
    }
    /// <summary>
    /// 確認死亡
    /// </summary>
    private void CheckDead()
    {
        if (healthcount <= 0)
        {
            playerCtrl.PlaySoundEffect(State.dead);
            playerCtrl.GetComponent<Animator>().SetTrigger("dead");
            if (transform.position.x > 0)
            {
                this.gameObject.GetComponent<Rigidbody>().AddForce(100000 * Time.deltaTime, 100000 * Time.deltaTime, 0);
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody>().AddForce(-100000 * Time.deltaTime, 100000 * Time.deltaTime, 0);
            }
            if (photonView.IsMine)
            {
                GameManager.instance.GameOver();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    /// <summary>
    /// 吃到道具補血
    /// </summary>
    private void AddHealth()
    {
        healthcount += 20;
        healthcount = Mathf.Clamp(healthcount, 0, 100);
        photonView.RPC("UpdateHealth", RpcTarget.All, RoomManager.localPlayerPos, healthcount, photonView.Owner);
    }
    /// <summary>
    /// 受傷
    /// </summary>
    private void Hurted()
    {
        if (hurtopen == true)
        {
            Invincible();
            playerCtrl.PlaySoundEffect(State.hurt);
            playerCtrl.AnimPlay(State.hurt);
            healthcount -= 20;
            healthcount = Mathf.Clamp(healthcount, 0, 100);
            photonView.RPC("UpdateHealth", RpcTarget.All, RoomManager.localPlayerPos, healthcount, photonView.Owner);
        }
    }
    /// <summary>
    /// 無敵
    /// </summary>
    private void Invincible()
    {
        ishurt.enabled = false;
        hurtopen = false;
    }
    /// <summary>
    /// 重置狀態
    /// </summary>
    public void ResetHurt()
    {
        anim.SetBool("hurt", false);
        if (inbubble == false)//沒有吃到道具無敵
        {
            ishurt.enabled = true;
            hurtopen = true;
        }
    }
    /// <summary>
    /// 道具結束
    /// </summary>
    public void InBubbleOver()
    {
        inbubble = false;
        ishurt.enabled = true;
        hurtopen = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (other.gameObject.tag == "deadzone")//直接死亡
        {
            healthcount = 0;
            photonView.RPC("UpdateHealth", RpcTarget.All, RoomManager.localPlayerPos, healthcount, photonView.Owner);
        }    
        if (other.gameObject.tag == "addhealth")
        {
            AddHealth();
        }
        if (hurtopen == false)
            return;
        switch (other.gameObject.tag)
        {
            case "thunder":
                if (other.transform.parent.GetComponent<PhotonView>().IsMine == false)
                {
                    Hurted();
                }
                break;
            case "bomb":
                if (transform.position.y > other.transform.position.y)
                {
                    this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(0, 100000 * Time.deltaTime, 0);
                    Hurted();
                }
                break;
            case "Player":
                if (other.transform.position.y > this.transform.position.y)
                {
                    Hurted();
                }
                break;
            case "headtab":
                if (this.transform.position.y > other.transform.position.y)
                {
                    Hurted();
                }
                break;
            case "gtskill":
                if (other.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    Hurted();
                }
                break;
            case "soliskill":
                if (other.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    Hurted();
                }
                break;
            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (collision.gameObject.tag == "bubble" || collision.gameObject.tag == "headtab")
        {
            inbubble = true;
            Invincible();
            Invoke("InBubbleOver", 5f);
        }
        if (collision.gameObject.tag == "soliskill" || collision.gameObject.tag == "njskill")
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false)
            {
                Hurted();
            }
        }
    }

    public void SlowTime()
    {
        Time.timeScale = 0.3f;
    }
    public void ResetTime()
    {
        Time.timeScale = 1f;
    }
}
