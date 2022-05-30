using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class addhealth : MonoBehaviourPun,IPunObservable
{
    public AudioSource audiosource;
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
        Invoke("de", 8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audiosource.Play();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    public void de()
    {
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
