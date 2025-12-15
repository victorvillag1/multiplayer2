using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviourPun
{
    public float speed = 5f;

    [Tooltip("Arrastra aqui la camara que esta dentro del Player")]
    public GameObject playerCamera;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        // Gestion de la Camara
        if (playerCamera != null)
        {
            // Solo activamos la camara si el jugador es "mio"
            // Si es de otro jugador, la desactivamos para no ver su punto de vista
            playerCamera.SetActive(photonView.IsMine);
        }
    }

    void Update()
    {
        // Solo controlamos a nuestro propio jugador
        if (photonView.IsMine)
        {
            // Leemos el valor de la acción "Move" definida en los Input Actions por defecto
            Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

            Vector3 move = new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.deltaTime;
            transform.Translate(move);
        }
    }
}