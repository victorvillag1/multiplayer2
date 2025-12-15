using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviourPun
{
    [Header("Ajustes")]
    public float speed = 5f;

    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto Cámara que está DENTRO de este Prefab")]
    [SerializeField] private GameObject playerCamera;

    private PlayerInput playerInput;
    private bool isMyCharacter; // Variable para almacenar el resultado de IsMine

    void Start()
    {
        // 1. Capturar el estado UNA VEZ en Start (es el estado correcto al instanciar)
        isMyCharacter = photonView.IsMine;

        // 2. Depuración: Ver quién soy y quién es mi dueño.
        Debug.Log($"[PlayerController] ViewID: {photonView.ViewID} - Soy mío: {isMyCharacter} - Dueño: {photonView.Owner.NickName}");

        if (!isMyCharacter)
        {
            // SI NO SOY MÍO: Desactivar Input y Camera/Audio
            
            // Si el objeto tiene un PlayerInput, lo desactivamos para que no reciba comandos
            if (playerInput == null) playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = false;

            if (playerCamera != null)
            {
                playerCamera.SetActive(false);
                var listener = playerCamera.GetComponent<AudioListener>();
                if (listener != null) listener.enabled = false;
            }
            
            // Ya que no es mío, no necesitamos seguir ejecutando el resto del Start()
            return;
        }

        // --- LÓGICA SOLO PARA MI PERSONAJE ---

        // 3. Activación de Input y Camera/Audio (si es mío)
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true; // Asegurarse de que el input está activo

        if (playerCamera != null)
        {
            playerCamera.SetActive(true);
            playerCamera.tag = "MainCamera";
            
            var listener = playerCamera.GetComponent<AudioListener>();
            if (listener != null) listener.enabled = true;
        }
    }

    void Update()
    {
        // El control de si es mío se hace con la variable almacenada UNA VEZ.
        if (!isMyCharacter) return; 

        // Aseguramos que el input esté disponible antes de leer
        if (playerInput != null && playerInput.actions["Move"] != null)
        {
            Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
            
            if (inputVector != Vector2.zero)
            {
                Vector3 move = new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.deltaTime;
                transform.Translate(move);
            }
        }
    }
}