using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organ : MonoBehaviour
{
    public AudioSource audioSource;
    public Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        coll = GetComponent<Collider>();
    }
    public virtual void Work(float cd)
    {
        audioSource.Play();
        coll.enabled = false;
        Invoke("ResetOrgan", cd);
    } 
    public virtual void ResetOrgan()
    {
        coll.enabled = true;
    }
}
