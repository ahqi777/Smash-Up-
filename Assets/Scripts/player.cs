using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
public class player : MonoBehaviourPun,IPunObservable
{
    /*float move ;
    Button solibutton;
    Button skillbutton;
    Joystick Joystick;*/
    GameObject soliarms;
    GameObject gtarms;
    public bool soliopen,soliatt,gtatt;
    public AudioSource jump,skillsource,hurtsound,deadsound;
    public Light skilllight1;
    public bool openskill;
    public bool checkground;
    bool skillopen;
    public Animator anim;
    public Animator skilllight;
    public int player1;
    public TMP_Text playername;
    public float speed;
    Vector3 currPos = Vector3.zero;
    Vector3 currScale = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    // Start is called before the first frame update
    private void Awake()
    {
        /*Joystick = GameObject.Find("joysticks").transform.GetChild(0).gameObject.GetComponent<Joystick>();
        solibutton = GameObject.Find("joysticks").transform.GetChild(1).gameObject.GetComponent<Button>();
        skillbutton = GameObject.Find("joysticks").transform.GetChild(2).gameObject.GetComponent<Button>();*/
        
        if (photonView.IsMine)
        {
            playername.text = PhotonNetwork.NickName;

        }
        else
        {
            playername.text = photonView.Owner.NickName;

        }
        currPos = transform.position;
        currScale = transform.localScale;
        currRot = transform.rotation;
    }
    void Start()
    {
        /*skillbutton.onClick.AddListener(skill);
        solibutton.onClick.AddListener(soliattack1);*/
        checkground = false;
        skillopen = false;
        player1 = RoomManager.roleIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (skilllight1.intensity == 40)
        {
            openskill = true;
         
            /*skillbutton.image.color = new Color(255,255,255,1);
            skillbutton.enabled = true;*/
        }
        else
        {
            openskill = false;

            /*skillbutton.image.color = new Color(255, 255, 255, 0.2f);
            skillbutton.enabled = false;*/
        }
        if (checkground == false)
        {
            anim.SetBool("checkground", checkground);
        }
        //move = Joystick.Horizontal;
        walk();
        skill();
        soliattack();
        gtattack();
        if (transform.position.y > 10f)
        {
            this.GetComponent<Collider>().attachedRigidbody.AddForce(0, -100000 * Time.deltaTime, 0);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Setstatus", RpcTarget.Others, transform.position, transform.localScale, transform.rotation);
        }
    }
    public void walk()
    {
        //transform.Translate(move*-speed * Time.deltaTime, 0, 0);
        if (Input.GetKey("left"))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("right"))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
    }
    public void soliattack()
    {
        if (soliopen)
        {
            Invoke("soliclose", 10f);
            if (soliatt == false)
            {
                soliarms.transform.position = new Vector3(transform.position.x + 0.8f, transform.position.y + 0.9f, transform.position.z);
                soliarms.GetComponent<Rigidbody>().freezeRotation = true;
            }
           
            if (Input.GetKey("left") && Input.GetKeyDown("r"))
            {
                soliarms.GetComponent<Rigidbody>().freezeRotation = false;
                soliatt = true;
                soliopen = false;
                soliarms.GetComponent<Rigidbody>().useGravity = true;
                soliarms.GetComponent<Collider>().attachedRigidbody.AddForce(-80000*Time.deltaTime, 500 * Time.deltaTime,0f);
                soliarms.transform.Rotate(0, 0, 500);
                Invoke("attsoli", 5f);
            }

            if (Input.GetKey("right") && Input.GetKeyDown("r"))
            {
                soliarms.GetComponent<Rigidbody>().freezeRotation = false;
                soliatt = true;
                soliopen = false;
                soliarms.GetComponent<Rigidbody>().useGravity = true;      
                soliarms.GetComponent<Collider>().attachedRigidbody.AddForce(80000 * Time.deltaTime, 500 * Time.deltaTime, 0f);
                soliarms.transform.Rotate(0, 0, -500 );
                Invoke("attsoli", 5f);
            }
        }
    }
    /*public void soliattack1()
    {
        if (soliopen)
        {
            Invoke("soliclose", 10f);
            if (soliatt == false)
            {
                soliarms.transform.position = new Vector3(transform.position.x + 0.8f, transform.position.y + 0.9f, transform.position.z);
                soliarms.GetComponent<Rigidbody>().freezeRotation = true;
            }

            if (move < 0)
            {
                soliarms.GetComponent<Rigidbody>().freezeRotation = false;
                soliatt = true;
                soliopen = false;
                soliarms.GetComponent<Rigidbody>().useGravity = true;
                soliarms.GetComponent<Collider>().attachedRigidbody.AddForce(-80000 * Time.deltaTime, 500 * Time.deltaTime, 0f);
                soliarms.transform.Rotate(0, 0, 500);
                Invoke("attsoli", 5f);
                solibutton.image.color = new Color(255, 255, 255, 0);
                solibutton.enabled = false;
            }

            if (move > 0)
            {
                soliarms.GetComponent<Rigidbody>().freezeRotation = false;
                soliatt = true;
                soliopen = false;
                soliarms.GetComponent<Rigidbody>().useGravity = true;
                soliarms.GetComponent<Collider>().attachedRigidbody.AddForce(80000 * Time.deltaTime, 500 * Time.deltaTime, 0f);
                soliarms.transform.Rotate(0, 0, -500);
                Invoke("attsoli", 5f);
                solibutton.image.color = new Color(255, 255, 255, 0);
                solibutton.enabled = false;
            }
        }
    }*/
    public void soliclose()
    {
        soliopen = false;
    }
    public void attsoli()
    {
        soliatt = false;
        soliarms.GetComponent<Rigidbody>().useGravity = false;
        PhotonNetwork.Destroy(soliarms);
    }
    public void gtattack()
    {
        if (skillopen==true)
        {
            if (gtatt==true)
            {
                gtarms.transform.position = transform.position;
            }
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
            Invoke("speedback", 3f);
        }
        if (other.gameObject.tag == "banana")
        {
            if (this.transform.position.y > other.transform.position.y)
            {
                if (this.transform.position.x > other.transform.position.x)
                {
                    this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(120000 * Time.deltaTime, 0, 0);
                    speed = 1;
                    Invoke("speedback", 3f);
                }
                else
                {
                    this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(-120000 * Time.deltaTime, 0, 0);
                    speed = 1;
                    Invoke("speedback", 3f);
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
        
        if (collision.gameObject.tag == "plane")
        {
            this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(0, 100000 * Time.deltaTime, 0);
        }
  
        if (collision.gameObject.tag == "headtab")
        {
            FindObjectOfType<playerhealth>().ishurt2();
            Invoke("rehurt", 5f);
        }
        if (collision.gameObject.tag == "bubble")
        {
            FindObjectOfType<playerhealth>().ishurt2();
            Invoke("rehurt", 5f);
        }
    }
    public void rehurt()
    {
        FindObjectOfType<playerhealth>().ishurt1();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            jump.Play();
            checkground = true;
            anim.SetBool("checkground", checkground);
            this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(0, 5000 * Time.deltaTime, 0);
        }
        else
        {
            checkground = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(0, 80000 * Time.deltaTime, 0);
        }
    }
    public void speedback()
    {
        speed = 3;
    }
    public void ground()
    {
        checkground = false;
        anim.SetBool("checkground", checkground);
    }
    public void skill()
    {
        if (openskill == true)
        {
            if (Input.GetKeyDown("r"))
            {
                if (this.gameObject.name == "soli")
                {
                    Invoke("soliskill", 1f);
                }
                if (this.gameObject.name == "nj")
                {
                    Invoke("njskill", 1f);
                }
                if (this.gameObject.name == "gt")
                {
                    Invoke("gtskill", 1.5f);
                }
                skillopen = true;
                anim.SetBool("skill", skillopen);
                skilllight.SetBool("skill", true);
            }             
        }
    }
    public void soliskill()
    {
        soliopen = true;
        /*solibutton.image.color = new Color(255, 255, 255, 1);
        solibutton.enabled = true;
        Invoke("soliattover",10f);*/
        soliarms = PhotonNetwork.Instantiate("soliarms", new Vector3(transform.position.x + 0.8f, transform.position.y + 0.9f, transform.position.z),Quaternion.identity);
    }
    /*public void soliattover()
    {
        solibutton.image.color = new Color(255, 255, 255, 0);
        solibutton.enabled = false;
    }*/
    public void njskill()
    {
        PhotonNetwork.Instantiate("njskill", transform.position, Quaternion.identity);
    }
    public void gtskill()
    {
        gtarms = PhotonNetwork.Instantiate("gtskill", transform.position, Quaternion.identity);
        gtatt = true;
        Invoke("gtskillover", 5f);
    }
    public void gtskillover()
    {
        gtatt = false;
    }
    public void hurtsource()
    {
        hurtsound.Play();
    }
    public void deadsource()
    {
        deadsound.Play();
    }
    public void skillssource()
    {
        skillsource.Play();
    }
    public void skillover()
    {
        skillopen = false;
        anim.SetBool("skill", skillopen);
        skilllight.SetBool("skill", false);
    }
    [PunRPC]
    void Setstatus(Vector3 newpos,Vector3 newrot,Quaternion quaternion)
    {
        currPos = newpos;
        currScale = newrot;
        currRot = quaternion;
    }
    public void slowskill()
    {
        Time.timeScale = 0.3f;
    }
    public void respeedskill()
    {
        Time.timeScale = 1f;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(checkground);
            stream.SendNext(skillopen);
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.rotation);
        }
        else 
        {
            checkground = (bool)stream.ReceiveNext();
            skillopen = (bool)stream.ReceiveNext();
            currPos = (Vector3)stream.ReceiveNext();
            currScale = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
