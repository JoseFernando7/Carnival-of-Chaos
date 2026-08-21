using UnityEngine;

public class UiWeapons : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    //public void SetAnimationInt(int i, int count)
    //{
    //    if(count == 0)
    //    {
    //        m_Animator.SetInteger("State", i);
    //    }
    //}
    public void ActivateShoe(int count)
    {
        if (count == 0)
        {
            m_Animator.SetTrigger("Shoe");
        }
    }

    public void ActivateCanon(int count)
    {
        if (count == 0)
        {
            m_Animator.SetTrigger("Canon");
        }
    }

    public void ActivateDog(int count)
    {
        if (count == 0)
        {
            m_Animator.SetTrigger("Dog");
        }
    }

    public void Desactivate()
    {
        m_Animator.SetTrigger("Desactivate");
    }
}
