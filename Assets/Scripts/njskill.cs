using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class njskill : MonoBehaviourPun,IPunObservable
{
    Vector3 currPos = Vector3.zero;
    Vector3 currScale = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    public float x, y,rota;
    public void Awake()
    {
        currPos = transform.position;
        currScale = transform.localScale;
        currRot = transform.rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(x * Time.deltaTime, y * Time.deltaTime, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.rotation.z > 230)
        {   
            this.gameObject.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 230f);
        }
        else if (this.gameObject.transform.rotation.z < -40)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, -40f);
        }
        else
        {
            this.gameObject.transform.Rotate(new Vector3(0, 0, rota));
        }  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PhotonView>().IsMine == false)
            {
                de();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            Invoke("de", 3f);
        }
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false)
            {
                de();
            }
        }
    }
    private void de()
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
