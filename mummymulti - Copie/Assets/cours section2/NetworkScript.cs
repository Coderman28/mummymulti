using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class NetworkScript : MonoBehaviour {


    [SerializeField]
    public Text TxtConnexionState, TxtAttente;

    [SerializeField]
    public Text TxtMyPlayerNickname;

    bool inRoom = false;
    public GameObject PlayerPrefab;

    [SerializeField]
    public GameObject SpawnPoint, PanelAttente;

    [SerializeField]
    Text playerList;

    [SerializeField]
    private Text TxtMasterClient;
    
	
	void Start () {
        
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("proto1");
        }

        PhotonNetwork.JoinLobby();
	}

    void Update()
    {
        TxtMasterClient.text = "is master client : " + PhotonNetwork.isMasterClient;
    }
	
	
	

    void OnJoinedLobby()
    {
        RoomOptions MyRoomOptions = new RoomOptions();
        MyRoomOptions.MaxPlayers = 2;
        PhotonNetwork.playerName = "Player" + Random.Range(1, 500);
        PhotonNetwork.JoinOrCreateRoom("The Game", MyRoomOptions, TypedLobby.Default);

        
        
        
    }

    public void UpdateListOfPlayers()
    {
        playerList.text = null;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            playerList.text += player.NickName + "  score : " + player.GetScore() + "\n";
        }
    }

    void OnPhotonPlayerConnected()
    {
        UpdateListOfPlayers();
        OnJoinedRoom();
    }

    void OnPhotonPlayerDisconnected()
    {
        UpdateListOfPlayers();
    }

    void OnJoinedRoom()
    {

        if (PhotonNetwork.room.PlayerCount < PhotonNetwork.room.MaxPlayers)
        {
            TxtAttente.text = "Attente de joueurs " + PhotonNetwork.room.PlayerCount + "/2";
            return;
        }
        else
        {
            PanelAttente.SetActive(false);
        }

        inRoom = true;
        TxtConnexionState.text = "Room : " + PhotonNetwork.room.Name;
        TxtMyPlayerNickname.text = PhotonNetwork.player.NickName;

        //Spawnpoint
        
        Vector3 sp;

        sp = new Vector3(
            SpawnPoint.transform.position.x + Random.Range(-4f, 4f),
            SpawnPoint.transform.position.y,
            SpawnPoint.transform.position.z);
        
        sp += SpawnPoint.transform.position;

        GameObject MyPlayer;
        MyPlayer = PhotonNetwork.Instantiate
            (PlayerPrefab.name, sp, Quaternion.identity, 0);
        MyPlayer.GetComponent<mummyController>().enabled = true;
        MyPlayer.GetComponentInChildren<Camera>().enabled = true;
        MyPlayer.GetComponentInChildren<Camera>().GetComponent<AudioListener>().enabled=true;
        UpdateListOfPlayers();

    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log("Le nouveau Master client est : " + newMasterClient.NickName);
    }
}
