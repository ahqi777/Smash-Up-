using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
public class playerhealth : MonoBehaviourPunCallbacks,IPunObservable
{
    public AudioSource AudioSource;
    public int healthcount;
    bool hurt1, addhealth1,inbubble;
    public bool hurtopen;
    public Animator anim;
    Animator lifeanim;
    GameObject deadmenu;
    int player1234;
    public static int life1,life2,life3,life4;
    public Collider ishurt;
    Vector3 currPos = Vector3.zero;
    Vector3 currScale = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    // Start is called before the first frame update
    private void Awake()
    {
        //设置传送数据类型 (同步属性)
        GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
        // 将photonView的observe属性设置为这个脚本
        //GetComponent<PhotonView>().ObservedComponents[0] = this;
        //设置player位置的初始值
        
        currPos = transform.position;
        currScale = transform.localScale;
        currRot = transform.rotation;

    }
    void Start()
    {
        
        healthcount = 100;
        hurtopen = true;
        hurt1 = false;
        addhealth1 = false;
        life1 = 100;
        life2 = 100;
        life3 = 100;
        life4 = 100;
        player1234 = RoomManager.localPlayerPos;
        deadmenu = GameObject.Find("deadmenu");
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        healthcount = Mathf.Clamp(healthcount, 0, 100);

        life1 = Mathf.Clamp(life1, 0, 100);
        life2 = Mathf.Clamp(life2, 0, 100);
        life3 = Mathf.Clamp(life3, 0, 100);
        life4 = Mathf.Clamp(life4, 0, 100);

        if (player1234 == 1)
        {
            dead();
        }
        if (player1234 == 2)
        {
            dead();
        }
        if (player1234 == 3)
        {
            dead();
        }
        if (player1234 == 4)
        {
            dead();
        }

    }
    public void dead()
    {
        if (healthcount <= 0)
        {
            deadmenu.GetComponent<Animator>().SetTrigger("dead");
            FindObjectOfType<player>().deadsource();
           
            if (transform.position.x > 0)
            {
                this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(100000 * Time.deltaTime, 100000 * Time.deltaTime, 0);
            }
            else
            {
                this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(-100000 * Time.deltaTime, 100000 * Time.deltaTime, 0);
            }
        }
    }
    
    public void ishurt1()
    {
        ishurt.enabled = true;
    }
    public void ishurt2()
    {
        ishurt.enabled = false;
    }
    public void hurt()
    {
        if (hurtopen == true)
        {
            AudioSource.Play();
            hurt1 = true;
            ishurt.enabled = false;
            anim.SetBool("hurt", hurt1);
            healthcount -= 20;
            anim.SetInteger("health", healthcount);
            //lifeanim.SetInteger("life", healthcount);
            hurtopen = false;
        }
    }
    public void rehurt()
    {
        hurt1 = false;
        ishurt.enabled = true;
        anim.SetBool("hurt", hurt1);
        hurtopen = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (other.gameObject.tag == "deadzone")
        {
            healthcount = 0;
            anim.SetInteger("health", healthcount);
        }
        if (other.gameObject.tag == "thunder")
        {
            if (inbubble == false)
            {
                if (other.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    hurt();
                }
            }
        }
        if (other.gameObject.tag == "bomb")
        {
            if (inbubble == false)
            {
                if (transform.position.y > other.transform.position.y)
                {
                    this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(0, 100000 * Time.deltaTime, 0);
                    hurt();
                }
            }
        }
        if (other.gameObject.transform.position.y > this.gameObject.transform.position.y && other.gameObject.tag == "Player")
        {
            if (inbubble == false)
            {
                hurt();
            }
        }
        if (other.gameObject.tag == "addhealth")
        {
            addhealth1 = true;
            anim.SetBool("addhealth", addhealth1);
            healthcount += 20;
            anim.SetInteger("health", healthcount);
            //lifeanim.SetInteger("life", healthcount);
        }
        if (other.gameObject.tag == "headtab")
        {
            if (inbubble == false)
            {
                if (this.transform.position.y > other.transform.position.y)
                {
                    hurt();
                }
            }
        }
        if (other.gameObject.tag == "gtskill")
        {
            if (inbubble == false)
            {
                if (other.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    hurt();
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
     
        if (collision.gameObject.tag == "soliskill")
        {
            if (inbubble == false)
            {
                if (collision.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    hurt();
                }
            }
        }
        if (collision.gameObject.tag == "njskill")
        {
            if (inbubble == false)
            {
                if (collision.gameObject.GetComponent<PhotonView>().IsMine == false)
                {
                    hurt();
                }
            }
        }
        if (collision.gameObject.tag == "bubble")
        {
            inbubble = true;
            Invoke("outbubble", 5f);
        }
    }
    public void outbubble()
    {
        inbubble = false;
    }
    public void health()
    {
        addhealth1 = false;
        anim.SetBool("addhealth", addhealth1);
    }
    public void nohurt()
    {
        anim.SetBool("nohurt", false);
    }
    public void slowskill()
    {
        Time.timeScale = 0.3f;
    }
    public void respeedskill()
    {
        Time.timeScale = 1f;
    }
    public void de()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //传送本地玩家的位置（和旋转等）信息
        if (stream.IsWriting)
        {
            stream.SendNext(healthcount);
            stream.SendNext(hurt1);
            stream.SendNext(addhealth1);
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.rotation);
        }
        else//接收远程玩家的位置信息
        {
            healthcount = (int)stream.ReceiveNext();
            hurt1 = (bool)stream.ReceiveNext();
            addhealth1 = (bool)stream.ReceiveNext();
            currPos = (Vector3)stream.ReceiveNext();
            currScale = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
