using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class headtab : MonoBehaviourPun,IPunObservable
{
    public AudioSource AudioSource;
    public float currttime;
    public Collider hurtcollider;
    public bool have, acitve;
    Transform player;
    Vector3 currPos = Vector3.zero;
    Vector3 currScale = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    public void Awake()
    {
        currPos = transform.position;
        currScale = transform.localScale;
        currRot = transform.rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        currttime = Time.time;
        Invoke("de10", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (have==true)
        {
            this.transform.position = new Vector3(player.position.x+0.06f, player.position.y + 0.21f, player.position.z);
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Player")
        {
            if (acitve == false)
            {
                AudioSource.Play();
                player = collision.gameObject.transform;
                acitve = true;
            }
            Invoke("havehurt", 0.5f);
            have = true;
            Invoke("de", 5f);
        }
    }
    public void havehurt()
    {
        hurtcollider.enabled = true;
    }
    public void de()
    {
        GetComponent<Rigidbody>().useGravity = true;
        have = false;
        acitve = false;
        hurtcollider.enabled = false;
        PhotonNetwork.Destroy(this.gameObject);
    }
    public void de10()
    {
        if (have == false)
        {
            if (currttime >= 10)
            {
                de();
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
           
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.rotation);
        }
        else
        {
          
            currPos = (Vector3)stream.ReceiveNext();
            currScale = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
