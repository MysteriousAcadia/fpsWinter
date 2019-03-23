using UnityEngine.Networking;
using UnityEngine;

public class HostGame : MonoBehaviour {



    [SerializeField] private uint roomSize = 6;
    [SerializeField] private string roomName="";
    private string roomPassword = "";
    private NetworkManager networkManager;



    public void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }



    public void SetRoomName(string _name)

    {

        roomName = _name;
        //roomPassword = _password;
       // roomSize = Convert.ToUInt32(_size, 10);
    }

    public void SetRoomPassword(string _password)
    {
        roomPassword = _password;
    }


    public void CreateRoomMethod(){
        if(!string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Creating Room: "+roomName+" with room for: "+roomSize+" players");
            networkManager.matchMaker.CreateMatch(roomName,roomSize,true,"","","",0,0,networkManager.OnMatchCreate);


        }
    }
}
