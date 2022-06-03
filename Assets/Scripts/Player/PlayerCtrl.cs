using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
public enum State
{
    jump,
    hurt,
    skill,
    dead,
}
public class PlayerCtrl : MonoBehaviourPun
{
    GameObject soliarms;
    GameObject gtarms;
    Rigidbody rb;
    AudioSource sound_effect;
    Animator anim;

    public bool SkillIsReady;
    public bool isSkilling;
    public bool isGameStart;

    public TMP_Text playerName;
    public float speed;
    public int skillCD;
    public Light skillLight;
    public AudioClip[] sound_effects;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float smoothTime = 0.15f;
    void Start()
    {
        photonView.RPC("SkillCooling", RpcTarget.All, photonView.Owner);
        rb = GetComponent<Rigidbody>();
        if (!photonView.IsMine)
            Destroy(rb);
        sound_effect = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (photonView.IsMine)
        {
            playerName.text = PhotonNetwork.NickName;
            playerName.color = Color.red;
        }
        else
        {
            playerName.text = photonView.Owner.NickName;

        }
    }
    private void FixedUpdate()
    {
        if (isGameStart && photonView.IsMine)
            rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    void Update()
    {
        if (photonView.IsMine == false)
        {
            return;
        }
        Move();
        Skill();
        WarriorAttack();
        GuitaristAttack();
    }
    public void Move()
    {
        Vector3 moveDir = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, 0).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * speed, ref smoothMoveVelocity, smoothTime);
    }
    public void WarriorAttack()
    {
        if (this.gameObject.name != "Warrior(Clone)")
        {
            return;
        }
        if (isSkilling)
        {
            soliarms.transform.position = new Vector3(transform.position.x + 0.8f, transform.position.y + 0.9f, transform.position.z);
            Rigidbody rb = soliarms.GetComponent<Rigidbody>();
            soliarms.GetComponent<Collider>().enabled = false;
            rb.freezeRotation = true;
            if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.J))
            {
                soliarms.GetComponent<Collider>().enabled = true;
                rb.freezeRotation = false;
                rb.useGravity = true;
                rb.AddForce(-30000 * Time.deltaTime, 0, 0);
                soliarms.transform.Rotate(0, 0, 500);
                photonView.RPC("SkillCooling", RpcTarget.All, photonView.Owner);
                isSkilling = false;
            }
            else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.J))
            {
                soliarms.GetComponent<Collider>().enabled = true;
                rb.freezeRotation = false;
                rb.useGravity = true;
                rb.AddForce(30000 * Time.deltaTime, 0, 0);
                soliarms.transform.Rotate(0, 0, -500);
                photonView.RPC("SkillCooling", RpcTarget.All, photonView.Owner);
                isSkilling = false;
            }
        }
    }
    public void GuitaristAttack()
    {
        if (this.gameObject.name != "Guitarist(Clone)")
        {
            return;
        }
        if (isSkilling == true)
        {
            gtarms.transform.position = transform.position;
        }
    }
   
    public void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (other.gameObject.tag == "addspeed")
        {
            speed = 6;
            Invoke("SpeedBack", 3f);
        }
        if (other.gameObject.tag == "banana")
        {
            if (this.transform.position.y > other.transform.position.y)
            {
                if (this.transform.position.x > other.transform.position.x)
                    rb.velocity += new Vector3(10 * Time.deltaTime, 0, 0);
                else
                    rb.velocity += new Vector3(-10 * Time.deltaTime, 0, 0);
                speed = 1;
                Invoke("SpeedBack", 3f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (collision.gameObject.tag == "plane")
        {
            PlaySoundEffect(State.jump);
            AnimPlay(State.jump);
            rb.velocity += new Vector3(0, 400 * Time.fixedDeltaTime, 0);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (collision.gameObject.tag == "plane")
        {
            anim.SetBool("jump", false);
        }
    }
    public void SpeedBack()
    {
        speed = 3;
    }
    public void Skill()
    {
        if (SkillIsReady == true && Input.GetKeyDown(KeyCode.J))
        {
            AnimPlay(State.skill);
        }
    }
    public void GuitaristSkill()
    {
        if (this.photonView.IsMine)
        {
            gtarms = PhotonNetwork.Instantiate("GuitaristSkill", transform.position, Quaternion.identity);
        }
        isSkilling = true;
    }
    /// <summary>
    /// 技能動畫播放結束事件
    /// </summary>
    /// <param name="roleName"></param>
    public void SkillOver(string roleName)
    {
        SkillIsReady = false;
        switch (roleName)
        {
            case "FatMan":
                photonView.RPC("SkillCooling", RpcTarget.All, photonView.Owner);
                break;
            case "Warrior":
                if (this.photonView.IsMine)
                    soliarms = PhotonNetwork.Instantiate("soliarms", new Vector3(transform.position.x + 0.8f, transform.position.y + 0.9f, transform.position.z), Quaternion.identity);
                isSkilling = true;
                break;
            case "Ninja":
                if (this.photonView.IsMine)
                    PhotonNetwork.Instantiate("NinjaSkill", transform.position, Quaternion.identity);
                photonView.RPC("SkillCooling", RpcTarget.All, photonView.Owner);
                break;
            case "Guitarist":
                PhotonNetwork.Destroy(gtarms);
                isSkilling = false;
                photonView.RPC("SkillCooling", RpcTarget.All, photonView.Owner);
                break;
            default:
                break;
        }
        anim.SetBool("skill", false);
    }

    /// <summary>
    /// 播放動畫
    /// </summary>
    /// <param name="State"></param>
    public void AnimPlay(State State)
    {
        string[] animstate = { "jump", "hurt", "skill" };
        int currentPriority = 0;
        for (int i = 0; i < animstate.Length; i++)//找優先級
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animstate[i]))
                currentPriority = i;
        }
        if (currentPriority < State.GetHashCode())//目前優先級小於當前要撥放的動畫
        {
            for (int i = 0; i < 3; i++)//重設狀態
            {
                anim.SetBool(animstate[i], false);
            }
        }
        anim.SetBool(animstate[State.GetHashCode()], true);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="soundEffect"></param>
    public void PlaySoundEffect(State soundEffect)
    {
        sound_effect.clip = sound_effects[soundEffect.GetHashCode()];
        sound_effect.Play();
    }
    /// <summary>
    /// 技能CD中
    /// </summary>
    /// <param name="cd"></param>
    /// <returns></returns>
    private IEnumerator SkillCooling(int cd, string roleName)
    {
        skillLight.intensity = 0;
        skillLight.color = Color.white;
        int i = 0;
        while (i <= cd)
        {
            skillLight.intensity += 0.2f;
            yield return new WaitForSeconds(1);
            i++;
        }
        switch (roleName)
        {
            case "FatMan(Clone)":
                skillLight.color = Color.red;
                break;
            case "Warrior(Clone)":
                skillLight.color = new Color32(0, 108, 255, 255);
                break;
            case "Ninja(Clone)":
                skillLight.color = new Color32(255, 0, 255, 255);
                break;
            case "Guitarist(Clone)":
                skillLight.color = Color.yellow;
                break;
            default:
                break;
        }
        SkillIsReady = true;
    }
    public void StartCooling()
    {
        StartCoroutine(SkillCooling(skillCD, this.gameObject.name));//開始計算技能CD
    }
    [PunRPC]
    public void SkillCooling(Player owner)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().Owner == owner)
            {
                players[i].GetComponent<PlayerCtrl>().StartCooling();
            }
        }
    }
}
