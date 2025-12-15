using Photon.Pun;
using UnityEngine;
using System.Collections;

// Cambiamos a MonoBehaviourPunCallbacks para escuchar eventos
public class GameSetupController : MonoBehaviourPunCallbacks 
{
    [SerializeField] private string playerPrefabName = "Player";

    // Este evento se activará si entramos a la escena antes de que la conexión termine de sincronizar
    public override void OnJoinedRoom()
    {
        Debug.Log("GameSetup: OnJoinedRoom disparado tardíamente. Instanciando ahora.");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Evitar doble spawn si se llama dos veces por accidente
        if (GameObject.Find(playerPrefabName + "(Clone)") != null) return;

        Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        PhotonNetwork.Instantiate(playerPrefabName, randomPosition, Quaternion.identity);
        
        Debug.Log("Jugador instanciado correctamente.");
    }
}