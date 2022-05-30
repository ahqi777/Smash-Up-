using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class BGmusic : MonoBehaviourPun
{
    //public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*public void clicksource()
    {
        audioSource.Play();
    }*/
}
