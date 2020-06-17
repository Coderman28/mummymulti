using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mummyController : MonoBehaviour {
    public float speed = 2f, force = 400f;
    private Animator anim;
    private Rigidbody rb;
    [SerializeField]
    bool isGrounded = false;

    [SerializeField]
    int score = 0;
    PhotonView view;

    [SerializeField]
    private AudioClip soundJump, soundStar;
    AudioSource audioS;


    void Awake () {

        anim=GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
        audioS = GetComponent<AudioSource>();
       	}
	
	
	void Update () {
      
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v > 0)
            {
                transform.Translate(Vector3.forward * v * speed * Time.deltaTime);
                anim.SetFloat("forward", v);
            }

            if (v < 0)
            {
                anim.SetFloat("forward", 0);
            }

            transform.Rotate(Vector3.up * h * speed * 40 * Time.deltaTime);
        }      
       

    private void FixedUpdate()
    {      
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                
                anim.SetTrigger("jump");
                rb.AddForce(Vector3.up * force);
                audioS.PlayOneShot(soundJump);
                Debug.Log(1);

        }   
       
    }
    private void OnTriggerStay()
    {
        isGrounded = true;        
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "star" && view.isMine)
        {
            //Destroy(other.gameObject);
            score ++;
            PhotonNetwork.player.SetScore(score);
            
            view.RPC("UpdateListScoreForAllPlayer", PhotonTargets.All);
            view.RPC("DestroyGoMasterClient", PhotonTargets.MasterClient, other.gameObject.name);
            audioS.PlayOneShot(soundStar);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Vector3 sp;

            GameObject SpawnPoint = GameObject.Find("spawnpointameObject");

            sp = new Vector3(
                SpawnPoint.transform.position.x + Random.Range(-4f, 4f),
                SpawnPoint.transform.position.y,
                SpawnPoint.transform.position.z);

            sp += SpawnPoint.transform.position;

            transform.position = SpawnPoint.transform.position;
        }
    }

    [PunRPC]
    void UpdateListScoreForAllPlayer()
    {
        GameObject.Find("networkmanager").GetComponent<NetworkScript>().UpdateListOfPlayers();
    }

    [PunRPC]
    void DestroyGoMasterClient(string obj)
    {

        if (GameObject.Find("stars").transform.childCount == 1)
        {
            view.RPC("EndOfGame", PhotonTargets.AllBuffered);
        }

        GameObject GoToDestroy = GameObject.Find(obj);
        PhotonNetwork.Destroy(GoToDestroy);
    }

    [PunRPC]
    private void EndOfGame()
    {
        PhotonNetwork.LoadLevel("gameover");
    }

    
}
