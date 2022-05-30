using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
public class ChooseRole : MonoBehaviourPunCallbacks
{
    public GameObject noice;
    public Button[] Buttons;
    public static int roleindex;
    // Start is called before the first frame update
    void Start()
    {
        roleindex = -1;
    }
    // Update is called once per frame
    void Update()
    {
        if (roleindex == -1)
        {
            noice.SetActive(true);
        }
        else
        {
            noice.SetActive(false);
        }
    }
    public void ChooseRole_Btn(int index)
    {
        roleindex = index;
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i == roleindex)
                Buttons[i].GetComponent<Image>().color = Color.white;
            else
                Buttons[i].GetComponent<Image>().color = Color.black;
        }
    }
    public void Ready()
    {
        if (roleindex == -1)
        {
            noice.SetActive(true);
        }
        else
        {
            ScenesManager.instance.GoLobby();
        }
    }
}
