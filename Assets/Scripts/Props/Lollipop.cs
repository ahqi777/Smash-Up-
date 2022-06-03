using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class Lollipop : Props
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            audioSource.Play();

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("Cure", transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
