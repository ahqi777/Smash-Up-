using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class organ : MonoBehaviourPun
{
    string organ1;
    public string[] organlist;
    public float currttime, delaytime;
    float nowtime;
    // Start is called before the first frame update

    void Start()
    {
        nowtime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("openorgan", 5f);
    }
    void openorgan()
    {
        organ1 = organlist[Random.Range(0, organlist.Length)];
        currttime = Time.time;
        if (currttime > nowtime)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(organ1, new Vector3(Random.Range(-12, 12), 10f, 0), Quaternion.identity);
                nowtime = currttime + delaytime;
            }
        }
    }
}
