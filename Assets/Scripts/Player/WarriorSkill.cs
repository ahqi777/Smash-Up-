using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class WarriorSkill : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().IsMine)
                return;
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = false;
            Invoke("Photon_Destroy", 2);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = false;
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
