using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class WarriorSkill : MonoBehaviourPun
{
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
