using UnityEngine;

public class WizardView : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo stateInfo;

    public Animator Anim => anim;
    public AnimatorStateInfo StateInfo => anim.GetCurrentAnimatorStateInfo(0);

    public void ManualStart()
    {
        anim = GetComponent<Animator>();
    }

    public void SetAnimation(string name, bool setBool)
    {
        anim.SetBool(name, setBool);
    }

    public void SetAnimTrigger(string name)
    {
        anim.SetTrigger(name);
    }
}