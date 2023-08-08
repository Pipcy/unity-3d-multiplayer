using UnityEngine;
using Unity.Netcode;

public class StartNetwork : MonoBehaviour
{
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}

