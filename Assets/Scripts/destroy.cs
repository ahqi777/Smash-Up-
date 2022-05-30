using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class destroy : MonoBehaviourPun
{
    
    public int delay;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("de", delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void de()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
