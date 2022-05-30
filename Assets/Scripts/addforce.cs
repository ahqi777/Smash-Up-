using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addforce : MonoBehaviour
{
    public AudioSource AudioSource;
    public Animator anim;
    bool open;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.transform.position.y > this.transform.position.y)
            {
                anim.SetBool("jump", true);
                other.GetComponent<Collider>().attachedRigidbody.AddForce(0, 60000 * Time.deltaTime, 0);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioSource.Play();
            this.gameObject.GetComponent<Collider>().enabled = false;
            Invoke("collon", 5f);
        }
    }
    public void collon()
    {
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
}
