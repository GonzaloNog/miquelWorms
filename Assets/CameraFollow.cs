using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidad de suavizado para el seguimiento
    [SerializeField] private float horizontalOffset = 2f; // Espacio adicional a los lados
    [SerializeField] private float verticalOffset = 2f; // Espacio adicional a los lados

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (LevelManager.instance.GetCombatManager().getTargetCam() != null)
        {
            // Obtiene el componente SpriteRenderer del personaje
            SpriteRenderer spriteRenderer = LevelManager.instance.GetCombatManager().getTargetCam().GetComponent<SpriteRenderer>();

            // Verifica si el personaje est� volteado en el eje X
            //bool isFlipped = spriteRenderer != null && spriteRenderer.flipX;
            bool isFlipped = true;
            // Calcula la direcci�n del movimiento bas�ndose en la orientaci�n del personaje
            float moveDirection = isFlipped ? -1f : 1f;

            // Ajusta la posici�n de la c�mara suavemente bas�ndose en la direcci�n del movimiento
            Vector3 desiredPosition = new Vector3(LevelManager.instance.GetCombatManager().getTargetCam().transform.position.x + moveDirection * horizontalOffset, LevelManager.instance.GetCombatManager().getTargetCam().transform.position.y + verticalOffset, transform.position.z);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;
        }

    }
    
}