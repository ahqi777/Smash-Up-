using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Banana : Props
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            coll.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.position.y > transform.position.y)
            {
                StopAllCoroutines();
                audioSource.Play();
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.Destroy(this.gameObject);
            }           
        }
    }
}
