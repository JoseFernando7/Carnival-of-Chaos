using UnityEditor.Animations;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [Header("Panel de Selección")]
    [SerializeField] private GameObject panelSeleccion;

    [Header("Skins de Jugador")]
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite skinPerezosa;
    [SerializeField] private RuntimeAnimatorController animatorPerezosa;
    [SerializeField] private Sprite skinPez;
    [SerializeField] private RuntimeAnimatorController animatorPez;
    [SerializeField] private Sprite skinOso;
    [SerializeField] private RuntimeAnimatorController animatorOso;
    [SerializeField] private Sprite skinZorro;
    [SerializeField] private RuntimeAnimatorController animatorZorro;

    private Animator _animator;
    private SpriteRenderer _renderer;


    // Método para seleccionar Pérezosa
    public void SeleccionarPerezosa()
    {
        ActivarSkin(skinPerezosa, animatorPerezosa);
    }

    // Método para seleccionar Pez
    public void SeleccionarPez()
    {
        ActivarSkin(skinPez, animatorPez);
    }

    // Método para seleccionar Oso
    public void SeleccionarOso()
    {
        ActivarSkin(skinOso, animatorOso);
    }

    // Método para seleccionar Zorro
    public void SeleccionarZorro()
    {
        ActivarSkin(skinZorro, animatorZorro);
    }

    // Apaga todas las skins, activa la elegida y oculta el panel
    private void ActivarSkin(Sprite sprite, RuntimeAnimatorController animatorController)
    {
        player.AddComponent<SpriteRenderer>();
        player.GetComponent<SpriteRenderer>().sprite = sprite;

        player.GetComponent<Animator>().runtimeAnimatorController = animatorController;


        // 3. Ocultar el panel de selección (esto desaparece todos los botones)
        if (panelSeleccion != null)
        {
            panelSeleccion.SetActive(false);
        }
    }
}
