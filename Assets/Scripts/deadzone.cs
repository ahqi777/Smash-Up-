using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

public class deadzone : MonoBehaviourPun
{
    int player1234;
    public GameObject deadmenu;
    // Start is called before the first frame update
    void Start()
    {
        player1234 = RoomManager.localPlayerPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            if (player1234 == 1)
            {

                deadmenu.GetComponent<Animator>().SetTrigger("dead");

            }
            if (player1234 == 2)
            {

                deadmenu.GetComponent<Animator>().SetTrigger("dead");

            }
            if (player1234 == 3)
            {

                deadmenu.GetComponent<Animator>().SetTrigger("dead");
            }
            if (player1234 == 4)
            {

                deadmenu.GetComponent<Animator>().SetTrigger("dead");

            }
        }
    }
}
