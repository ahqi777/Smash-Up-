using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class RandomProps : MonoBehaviourPun
{
    public List<string> props = new List<string>();
    public float[] propsProbs;
    public float propsRandomTime;
    public int amount;
    private float propsCountTime;
    private Vector3 randomPosition;

    void Start()
    {
        randomPosition = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            Handle(ref propsCountTime, propsRandomTime, propsProbs, props, amount);
    }
    void Handle(ref float countTime, float randomTime, float[] probs, List<string> gameObjects, int amount)
    {
        countTime += Time.deltaTime;
        if (countTime >= randomTime)
        {
            for (int i = 0; i < amount; i++)
            {
                randomPosition.x = Random.Range(-12, 12);
                int index = Choose(probs);
                PhotonNetwork.Instantiate(gameObjects[index], randomPosition, Quaternion.identity);
            }
            countTime = 0;
        }
    }
    /// <summary>
    /// 隨機選擇
    /// </summary>
    /// <param name="probs"></param>
    /// <returns></returns>
    int Choose(float[] probs)
    {
        float total = 0;

        foreach (float item in probs)
        {
            total += item;
        }
        float randomPoint = Random.value * total;
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
