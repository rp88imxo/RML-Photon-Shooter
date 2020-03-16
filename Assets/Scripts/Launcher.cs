using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region PRIVATE_SERIAL_FIELDS
    [Tooltip("Maximum number of players per room"), SerializeField]
    private byte maxPlayerPerRoom = 4;

    [SerializeField]
    GameObject controlPanel;

    [SerializeField]
    GameObject progressLabel;

    #endregion

    #region PRIVATE_FIELDS

    string gameVersion = "0.0.1";
    bool isConnecting;

    #endregion

    #region MONOBEH_CALLBACKS

  
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region MONOBEH_PUN_CALLBACKS
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected From Master Server!");
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("Connected To Maser Server!");
            Join();
            isConnecting = false;
        }
      
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Success on room join!");
        GameStart();


    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("There is no rooms! Create the new one...");
        Create();

    }


    #endregion


    #region MINE_FUNCTIONS
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            Join();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            isConnecting = PhotonNetwork.ConnectUsingSettings();
        }


    }

    private void Create()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayerPerRoom });
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void GameStart()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Load level for one player...");

            PhotonNetwork.LoadLevel("GameScene1");
            
        }

    }
    #endregion

}
