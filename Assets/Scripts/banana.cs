using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class banana : MonoBehaviourPun,IPunObservable
{
    public AudioSource AudioSource;
    public Collider collider1;
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
        Invoke("de", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            collider1.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.position.y > transform.position.y)
            {
                AudioSource.Play();
                Invoke("de", 0.5f);
            }           
        }
    }
    public void de()
    {
        collider1.isTrigger = false;
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
