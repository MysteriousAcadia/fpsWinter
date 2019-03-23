using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
public class RoomListItem : MonoBehaviour {

    [SerializeField] private Text roomNameText;
    private MatchInfoSnapshot match;


    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);

    public JoinRoomDelegate joinRoomCallBack;

    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallBack)
    {
        match = _match;
        joinRoomCallBack = _joinRoomCallBack;

        roomNameText.text = _match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    } 



    public void JoinRoom()
    {
        joinRoomCallBack.Invoke(match);
    }
	
}
