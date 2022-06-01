using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class NinjaSkill : MonoBehaviourPun
{
    public float x, y,rota;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(x * Time.deltaTime, y * Time.deltaTime, 0);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
            Invoke("Photon_Destroy", 2);
        }
    }
    private void Photon_Destroy()
    {
        if (this.photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
