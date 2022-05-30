using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class soliskill : MonoBehaviourPun,IPunObservable
{
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
        Invoke("de", 10f);   
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PhotonView>().IsMine == false)
            {
                Invoke("de", 3f);
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            Invoke("de", 3f);
        }
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false)
            {
                Invoke("de", 3f);
            }
        }
    }
    public void de()
    {
        GetComponent<Rigidbody>().useGravity = false;
        PhotonNetwork.Destroy(this.gameObject);
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
