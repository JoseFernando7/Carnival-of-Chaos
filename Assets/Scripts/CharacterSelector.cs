using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [Header("Panel de Selección")]
    [SerializeField] private GameObject panelSeleccion;

    [Header("Skins de Jugador")]
    [SerializeField] private GameObject player;

    [Header("Sonidos Comunes")]
    [SerializeField] private AudioClip clipImpactoComun;
    [SerializeField] private AudioClip clipPasosComun;

    [Header("Perezosa")]
    [SerializeField] private Sprite skinPerezosa;
    [SerializeField] private RuntimeAnimatorController animatorPerezosa;
    [SerializeField] private AudioClip sonidoPerezosa;

    [Header("Pez")]
    [SerializeField] private Sprite skinPez;
    [SerializeField] private RuntimeAnimatorController animatorPez;
    [SerializeField] private AudioClip sonidoPez;

    [Header("Oso")]
    [SerializeField] private Sprite skinOso;
    [SerializeField] private RuntimeAnimatorController animatorOso;
    [SerializeField] private AudioClip sonidoOso;

    [Header("Zorro")]
    [SerializeField] private Sprite skinZorro;
    [SerializeField] private RuntimeAnimatorController animatorZorro;
    [SerializeField] private AudioClip sonidoZorro;

    public void SeleccionarPerezosa() => ActivarSkin(skinPerezosa, animatorPerezosa, sonidoPerezosa);
    public void SeleccionarPez() => ActivarSkin(skinPez, animatorPez, sonidoPez);
    public void SeleccionarOso() => ActivarSkin(skinOso, animatorOso, sonidoOso);
    public void SeleccionarZorro() => ActivarSkin(skinZorro, animatorZorro, sonidoZorro);

    private void ActivarSkin(Sprite sprite, RuntimeAnimatorController animatorController, AudioClip audioAnimal)
    {
        if (player.GetComponent<SpriteRenderer>() == null)
        {
            player.AddComponent<SpriteRenderer>();
        }
        player.GetComponent<SpriteRenderer>().sprite = sprite;

        if (player.GetComponent<Animator>() != null)
        {
            player.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        }

        // Configuración de Audio Dinámico
        CharacterAudio charAudio = player.GetComponent<CharacterAudio>();
        if (charAudio == null)
        {
            charAudio = player.AddComponent<CharacterAudio>();
        }

        charAudio.clipAnimal = audioAnimal;
        charAudio.clipImpacto = clipImpactoComun;
        charAudio.clipPasos = clipPasosComun;
        charAudio.IniciarRuiditosAnimal();

        if (panelSeleccion != null)
        {
            panelSeleccion.SetActive(false);
        }
    }
}