using System;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public event Action AttackUsed;

    public abstract void Activate();

    protected void NotifyAttackUsed()
    {
        AttackUsed?.Invoke();
    }
}
