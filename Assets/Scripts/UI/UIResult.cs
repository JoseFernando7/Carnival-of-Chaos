using UnityEngine;

public class UIResult : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void ActivateAttackResult()
    {
        _animator.SetTrigger("Attack");
    }

    public void ActivateDrawResult()
    {
        _animator.SetTrigger("Draw");
    }

    public void ActivateRunResult()
    {
        _animator.SetTrigger("Run");
    }
}
