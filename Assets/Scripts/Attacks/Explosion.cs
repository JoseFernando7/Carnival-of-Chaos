using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float duration = 0.5f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterDuration());
    }

    private IEnumerator DisableAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
