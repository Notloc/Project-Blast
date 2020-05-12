using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator = null;

    public void OpenContainer()
    {
        animator.SetBool("open", true);
    }

    public void CloseContainer()
    {
        animator.SetBool("open", false);
    }
}
