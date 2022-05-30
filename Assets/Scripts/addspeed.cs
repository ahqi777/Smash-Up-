using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addspeed : MonoBehaviour
{
    public AudioSource AudioSource;
    public Collider coll; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -3.04f, this.gameObject.transform.position.z);
            AudioSource.Play();
            coll.enabled = false;
            Invoke("reposition", 3f);
        }
    }
    void reposition()
    {
        coll.enabled = true;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -2.8f, this.gameObject.transform.position.z);
    }
}
