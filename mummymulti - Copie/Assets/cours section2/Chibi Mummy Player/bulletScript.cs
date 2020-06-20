using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{

    [SerializeField]
    private float AutoDestroy = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, AutoDestroy);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int id = collision.gameObject.GetPhotonView().viewID;
            collision.gameObject.GetComponent<PhotonView>().RPC("BulletTouch", PhotonPlayer.Find(id));
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
