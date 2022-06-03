using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Spikecap : Props
{
    public bool have;
    Transform playerPosition;
    public BoxCollider box;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (have == true)
        {
            this.transform.position = new Vector3(playerPosition.position.x+0.06f, playerPosition.position.y + 0.21f, playerPosition.position.z);
            GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "Player")
        {
            if (have == false)
            {
                StopAllCoroutines();
                audioSource.Play();
                playerPosition = collision.gameObject.transform;
                coll.enabled = true;
                box.enabled = false;
                have = true;
                StartCoroutine(InPropsOver(5, collision.transform.GetComponent<PlayerHealth>()));
            }
        }
    }
}
