public class RoleFSMStateMachine : YogiGameCore.FSM.FSMSystem
{
    public RoleFSMStateMachine(Role role)
    {
        this.Init<Idle, RoleBasicState>();
        foreach (var state in _allFsmStates.Values)
        {
            (state as RoleBasicState).Init(role);
        }
    }
}
