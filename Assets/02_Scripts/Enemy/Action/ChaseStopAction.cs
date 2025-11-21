using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseStop", story: "[Self] Chase", category: "Action", id: "d099b4e562aafc058bce4966dc7ef413")]
public partial class ChaseStopAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private Animator animator;

    protected override Status OnStart()
    {
        if (Self.Value != null)
        {
            animator = Self.Value.GetComponent<Animator>();
            animator?.SetBool("isRunning", false);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

