using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class BGmusic : MonoBehaviourPun
{
    public GameObject bgm;
    void Start()
    {
        Time.timeScale = 1;
        if (!GameObject.Find(bgm.name+"(Clone)"))
        {
            GameObject temp = Instantiate(bgm, transform.position, Quaternion.identity);
            DontDestroyOnLoad(temp);
        }
    }
}
