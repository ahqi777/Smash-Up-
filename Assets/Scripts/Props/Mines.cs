using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class Mines : Props
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            coll.isTrigger = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.transform.position.y > transform.position.y)
        {
            StopAllCoroutines();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Explosion", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
