using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Spring : Organ
{
    public Animator anim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && other.transform.position.y > this.transform.position.y)
        {
            anim.SetBool("jump", true);
            if (other.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<Rigidbody>().AddForce(Vector3.up * 450000 * Time.deltaTime);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Work(5f);
        }
    }
    public override void ResetOrgan()
    {
        base.ResetOrgan();
        anim.SetBool("jump", false);
    }
}
