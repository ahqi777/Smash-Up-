using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Bubble : Props
{
    bool have;
    Transform playerPosition;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (have == true)
        {
            transform.position = new Vector3(playerPosition.position.x,playerPosition.position.y,playerPosition.position.z);
            transform.localScale = new Vector3(2f, 2f, 2f);
            GetComponent<Rigidbody>().useGravity = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (have == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                StopAllCoroutines();
                audioSource.Play();
                have = true;
                coll.isTrigger = true;
                playerPosition = collision.gameObject.transform;
                StartCoroutine(InPropsOver(5, collision.transform.GetComponent<PlayerHealth>()));
            }
        }
    }
}
