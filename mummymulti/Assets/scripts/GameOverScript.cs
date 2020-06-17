using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{

    [SerializeField]
    private Text txtLeaderBoard;

    void Start()
    {
        txtLeaderBoard.text = null;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            txtLeaderBoard.text += player.NickName + " Score : " + player.GetScore();
        }
        PhotonNetwork.LeaveRoom();
    }

    public void Rejouer()
    {
        PhotonNetwork.player.SetScore(0);
        PhotonNetwork.LoadLevel(0);
    }

}
