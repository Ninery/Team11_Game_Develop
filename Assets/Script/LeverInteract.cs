using UnityEngine;
using System.Collections;

public class LeverInteract : MonoBehaviour, IInteractable
{
    public Animator leverAnim;
    public string leverStateName = "LeverPull";

    private bool isDown = false;
    private bool isAnimating = false;

    public bool IsDown()
    {
        return isDown;
    }

    public void Interact()
    {
        if (isAnimating) return;

        isDown = !isDown;

        if (isDown)
        {
            isAnimating = true;
            leverAnim.speed = 1f;
            leverAnim.Play(leverStateName, 0, 0f);
        }
        else
        {
            StartCoroutine(PlayReverse());
        }
    }

    void Update()
    {
        if (!isAnimating || !isDown) return;

        AnimatorStateInfo state = leverAnim.GetCurrentAnimatorStateInfo(0);

        if (state.normalizedTime >= 1f)
            isAnimating = false;
    }

    private IEnumerator PlayReverse()
    {
        isAnimating = true;
        leverAnim.speed = 0f;

        float clipLength = leverAnim.GetCurrentAnimatorStateInfo(0).length;
        float t = 1f;

        while (t > 0f)
        {
            leverAnim.Play(leverStateName, 0, t);
            yield return null;
            t -= Time.deltaTime / clipLength;
        }

        leverAnim.Play(leverStateName, 0, 0f);
        isAnimating = false;
    }
}