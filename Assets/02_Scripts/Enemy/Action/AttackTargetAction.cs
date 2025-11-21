using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Self] Attack [Target] with [AttackRange]", category: "Action", id: "00154ab1fed5584a589b7e11cba3cfad")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> AttackRange;

    private Animator animator;

    protected override Status OnStart()
    {
        if (Self.Value != null)
            animator = Self.Value.GetComponent<Animator>();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value == null || Target.Value == null)
            return Status.Failure;

        //타겟이 공격 범위 박이면 공격안해야
        float distance = Vector2.Distance(Self.Value.transform.position, Target.Value.transform.position);

        if (distance > AttackRange.Value)
            return Status.Failure;

        if (animator != null)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            //공격 중이면 Running 유지 Success 반환하면 안댐
            if (state.IsTag("Attack"))
                return Status.Running;

            //공격모션
            animator.SetTrigger("attackTrigger");
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // 공격 종료 시 필요한 것 여기에
    }
}

