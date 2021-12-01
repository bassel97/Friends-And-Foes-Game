using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract void UpdateAction(StateController controller);
    public abstract void FixedUpdateAction(StateController controller);
}
