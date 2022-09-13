using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{

    
    #region MonoBehaviour
        void Start()
        {
            PhotonNetwork.OfflineMode = false;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = Random.Range(1000000000, 9999999999).ToString();

            JoinOnline();
        }

        /// <summary>
        /// Simulate connect to play offline
        /// </summary>
        public void JoinOffline() 
        {
            PhotonNetwork.OfflineMode = true;
        }
        
        /// <summary>
        /// Connect to master 
        /// </summary>
        public void JoinOnline() 
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    #endregion
    
    

    #region PunCallbacks
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        GameObject playerLobbyInstanceGO = PhotonNetwork.Instantiate("PlayerInstance", transform.position, transform.rotation);
    }
    #endregion

}