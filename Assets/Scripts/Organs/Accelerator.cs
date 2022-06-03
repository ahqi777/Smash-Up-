using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : Organ
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Work(5f);
        }
    }
    public override void Work(float cd)
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -3.04f, this.gameObject.transform.position.z);
        base.Work(cd);
    }
    public override void ResetOrgan()
    {
        base.ResetOrgan();
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -2.8f, this.gameObject.transform.position.z);
    }
}
