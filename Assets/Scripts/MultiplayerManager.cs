using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // 1. Conectar al servidor usando la configuración (AppId)
        Debug.Log("Conectando al servidor...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Callback cuando nos conectamos al servidor Master
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor Master");
        Debug.Log("Región: " + PhotonNetwork.CloudRegion);

        // Empezar a mostrar el ping
        StartCoroutine(PingLoop());

        // 2. Unirse a una sala aleatoria o crear una
        PhotonNetwork.JoinRandomRoom();
    }

    private IEnumerator PingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("Ping: " + PhotonNetwork.GetPing() + " ms");
            }
        }
    }

    // Callback si falla la unión aleatoria (porque no hay salas)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Fallo al unirse a sala aleatoria, creando una nueva...");
        // Crear una sala con opciones (ej. max 4 jugadores)
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(null, options);
    }

    // Callback cuando nos unimos exitosamente a una sala
    public override void OnJoinedRoom()
    {
        Debug.Log("¡Unido a la sala: " + PhotonNetwork.CurrentRoom.Name + "!");

        // Crear el jugador en una posición aleatoria (para evitar superposiciones)
        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        // NOTA: El prefab debe estar en una carpeta llamada "Resources"
        PhotonNetwork.Instantiate("Prefab_Player", randomPosition, Quaternion.identity);
    }
}