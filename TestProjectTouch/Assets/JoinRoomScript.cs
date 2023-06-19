using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class JoinRoomScript : MonoBehaviourPunCallbacks
{
    
    public TMPro.TMP_InputField createInput;
    public TMPro.TMP_InputField joinField;
    public void CreateRm()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    
    }

    public void JoinRm()
    {
        PhotonNetwork.JoinRoom(joinField.text);
    }
    // Start is called before the first frame update

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("PaintTestExport");
    }
}
