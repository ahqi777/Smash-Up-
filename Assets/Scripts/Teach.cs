using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teach : MonoBehaviour
{
    public GameObject leftbutton, rightbutton, start;
    public Sprite[] sprites;
    public Image image;
    int index;
    private void Awake()
    {
        if (GameObject.Find("BGmusic"))
        {
            Destroy(GameObject.Find("BGmusic"));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (index == 2)
        {
            rightbutton.SetActive(false);
            start.SetActive(true);
        }
        else
        {
            rightbutton.SetActive(true);
            start.SetActive(false);
        }
        if (index == 0)
        {
            leftbutton.SetActive(false);
        }
        else
        {
            leftbutton.SetActive(true);
        }
    }
    public void right()
    {
        index += 1;

        image.sprite = sprites[index];
    }
    public void left()
    {
        index -= 1;
        image.sprite = sprites[index];
    }
}
