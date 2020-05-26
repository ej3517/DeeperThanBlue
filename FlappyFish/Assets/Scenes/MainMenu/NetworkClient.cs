using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

namespace Project.Networking {
    public class NetworkClient : SocketIOComponent
{

    [Header("Network Client")]
    [SerializeField]
    private Transform networkContainer; 
    [SerializeField]
    private GameObject playerPrefab; 

    public static string ClientID {get; private set; }
  

    // Start is called before the first frame update
    public override void Start()
    {
        // Add code below base.
        base.Start();
        initialize(); 
        setupEvents();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void initialize(){
        //serverObjects = new Dictionary<string, NetworkIdentity>(); 
    }

    private void setupEvents()
    {
        On("open", (E) => 
        {
            Debug.Log("Connection made to the server"); 
        }); 

        On("register", (E) => 
        {
            string id = E.data["id"].ToString().Replace("\"", ""); 
            Debug.LogFormat("Our Client's ID is ({0})", id);
        });

    }
}

[System.Serializable]
public class Player 
{
    public string id; 
    public Position position; 
}

[System.Serializable]
public class Position
{
    public float x; 
    public float y; 
}

}


