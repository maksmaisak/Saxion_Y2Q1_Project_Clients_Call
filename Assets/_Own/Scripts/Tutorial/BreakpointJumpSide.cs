using UnityEngine;

public class BreakpointJumpSide : TutorialBreakpoint
{
    [SerializeField] bool jumpRight;
    
    protected override bool ReleaseCondition()
    {
        if (!Input.GetButtonDown("Horizontal")) return false;
        
        if (jumpRight) return Input.GetAxisRaw("Horizontal") > 0f;
        return Input.GetAxisRaw("Horizontal") < 0f;
    }
}