using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class name : MonoBehaviourPunCallbacks
{
    public TMP_Text TMP_Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Awake()
    {
        if (photonView.IsMine)
        {
            TMP_Text.text = PhotonNetwork.NickName;
        }
        else
        {
            TMP_Text.text = photonView.Owner.NickName;
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
