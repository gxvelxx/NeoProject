using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsInAttackRange", story: "[Self] inRange [Target] with [AttackRange]", category: "Action", id: "ed290eb0ed34d523324dbf4d8eb46794")]
public partial class IsInAttackRange : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> AttackRange;
    protected override Status OnUpdate()
    {
        if (Self.Value == null || Target.Value == null)
            return Status.Failure;

        float distance = Vector2.Distance(Self.Value.transform.position, Target.Value.transform.position);

        return distance <= AttackRange.Value ? Status.Success : Status.Failure;
    }
}

