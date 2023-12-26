using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonDetect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool botonPresionado = false;
    public int move = 1;
    public string mode = "move";

    public void OnPointerDown(PointerEventData eventData)
    {
        // El botón se ha presionado
        botonPresionado = true;
        switch (mode)
        {
            case "finger":
                LevelManager.instance.GetCombatManager().fingerAndroid();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        botonPresionado = false;
        switch (mode)
        {
            case "move":
                LevelManager.instance.GetCombatManager().stopMoveWorm();
                break;
            case "cargar":
                LevelManager.instance.GetCombatManager().shootWorm();
                break;
        }
    }

    void Update()
    {
        if (botonPresionado)
        {
            switch (mode)
            {
                case "move":
                    LevelManager.instance.GetCombatManager().moveWorm(move);
                    break;
                case "apuntar":
                    LevelManager.instance.GetCombatManager().apuntarWorm(move);
                    break;
                case "cargar":
                    LevelManager.instance.GetCombatManager().shootChangeWorm();
                    break;
            }
        }
    }
}
