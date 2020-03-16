using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region PUBLIC_PROP

    public Camera additionalCamera;

    public string playerPrefab;
    public Transform spawnPos;

    public GameObject GameMenu;
    #endregion



    #region PUBLIC_STATIC_PROP

    public static bool isGamePaused = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (GameMenu)
        {
            GameMenu.SetActive(true);
        }
        
        Spawn();
    }

    private void Spawn()
    {
        PhotonNetwork.Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LeaveRoom();
        }
        
    }

    #region PHOTON_CALLBACKS

    public override void OnLeftRoom()
    {
        Loader.Load(Loader.Scene.MenuScene);
    }

    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        Debug.LogFormat("OnPlayerEnteredRoom: {0}", newPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("IsMasterClient {0} Nickname: {1}", PhotonNetwork.IsMasterClient, newPlayer.NickName);
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("{0} has been disconnected from the room!", otherPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("Master client {0} has left the room!", PhotonNetwork.NickName); // called before OnPlayerLeftRoom
            

            LoadArena();
        }
    }

    #endregion



    #region PUBLIC_METHODS
    public static void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region PRIVATE_METHODS
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Cant load level since you are not a Master Client!");
        }
        Debug.LogFormat("Loading Level: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("GameScene" + PhotonNetwork.CurrentRoom.PlayerCount);

    }
    #endregion
}
