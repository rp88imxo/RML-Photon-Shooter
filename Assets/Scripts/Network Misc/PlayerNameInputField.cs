using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;



public class PlayerNameInputField : MonoBehaviour
{
	#region PRIVATE_CONSTANTS
	const string playerNamePrefKey = "Player";
	#endregion

	#region MONO_CALLBACKS

	
	private void Start()
	{
		string defaultName = string.Empty;
		TMPro.TMP_InputField IField = GetComponent<TMPro.TMP_InputField>();
		
		if (IField != null)
		{
			if (PlayerPrefs.HasKey(playerNamePrefKey))
			{
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				IField.text = defaultName;
			}
		}

		PhotonNetwork.NickName = defaultName;
	}
	#endregion

	#region PUBLIC_METHODS
	/// <summary>
	/// Set name of the player and stores to PlayerPref for future sessions
	/// </summary>
	/// <param name="value"></param>
	public void SetPlayerName(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			Debug.LogError("Player Name can't be null or empty");
			return;
		}
		PhotonNetwork.NickName = value;

		PlayerPrefs.SetString(playerNamePrefKey, value);
	}
	#endregion
}
