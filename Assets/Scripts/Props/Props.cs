using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Props : MonoBehaviourPun
{
    public AudioSource audioSource;
    public Collider coll;
    public int lifeTime;
    public virtual void Start()
    {
        StartCoroutine(Photon_Destroy(lifeTime));
    }
    /// <summary>
    /// 道具存在時間
    /// </summary>
    /// <param name="life"></param>
    /// <returns></returns>
    public virtual IEnumerator Photon_Destroy(int life)
    {
        int i = 0;
        while (i < life)
        {
            yield return new WaitForSeconds(1f);
            i++;
        }
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
    }
    /// <summary>
    /// 道具使用時間
    /// </summary>
    /// <param name="life">持續時間</param>
    /// <param name="playerHealth"></param>
    /// <returns></returns>
    public IEnumerator InPropsOver(int life, PlayerHealth playerHealth)
    {
        int i = 0;
        while (i < life)
        {
            yield return new WaitForSeconds(1);
            i++;
        }
        playerHealth.InBubbleOver();
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(this.gameObject);
    }
}
