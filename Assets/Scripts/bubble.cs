using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class bubble : MonoBehaviourPun,IPunObservable
{
    public AudioSource AudioSource;
    public float currttime;
    bool inside;
    public Collider coll;
    Transform playerposition;
    Vector3 currPos = Vector3.zero;
    Vector3 currScale = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    // Start is called before the first frame update
    public void Awake()
    {
        currPos = transform.position;
        currScale = transform.localScale;
        currRot = transform.rotation;
    }
    void Start()
    {
        currttime = Time.time;
        Invoke("de10", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inside == true)
        {
            this.transform.position = new Vector3(playerposition.position.x,playerposition.position.y,playerposition.position.z);
            this.transform.localScale = new Vector3(2f, 2f, 2f);
            GetComponent<Rigidbody>().useGravity = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (inside == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                AudioSource.Play();
                inside = true;
                coll.isTrigger = true;
                playerposition = collision.gameObject.transform;
                Invoke("de", 5f);
            }
        }
    }
    public void de()
    {
        GetComponent<Rigidbody>().useGravity = true;
        coll.isTrigger = false;
        inside = false;
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void de10()
    {
     
        if (inside == false)
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
