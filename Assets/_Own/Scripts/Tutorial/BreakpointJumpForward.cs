using UnityEngine;

public class BreakpointJumpForward : TutorialBreakpoint
{
    protected override bool ReleaseCondition()
    {
        return Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0f;
    }
}