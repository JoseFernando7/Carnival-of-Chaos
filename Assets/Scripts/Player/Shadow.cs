using UnityEngine;


public class Shadow : MonoBehaviour
{
    [SerializeField] private string targetTag;
    private GameObject target;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _spriteRenderer.sprite = target.GetComponent<SpriteRenderer>().sprite;
    }


}
