using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChaseMove", story: "[Self] Chase [Target] with [MoveSpeed] and [AttackRange]", category: "Action", id: "57d4db1355ccc27202b9e0097606d508")]
public partial class ChaseMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    [SerializeReference] public BlackboardVariable<float> AttackRange;

    private Animator animator;

    protected override Status OnStart()
    {
        if (Self.Value != null)
        {
            animator = Self.Value.GetComponent<Animator>();
            animator?.SetBool("isRunning", true);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value == null || Target.Value == null)
            return Status.Failure;

        // 공격범위에 들어왔다면 추적 종료 → Success
        float distance = Vector2.Distance(Self.Value.transform.position, Target.Value.transform.position);
        if (distance <= AttackRange.Value)
        {
            animator?.SetBool("isRunning", false);
            return Status.Success;
        }

        // 공격 중이면 이동 중단
        if (animator != null)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsTag("Attack"))
            {
                animator.SetBool("isRunning", false);
                return Status.Running;
            }
        }

        // 이동
        Vector3 dir = (Target.Value.transform.position - Self.Value.transform.position).normalized;
        Self.Value.transform.position += dir * MoveSpeed.Value * Time.deltaTime;

        animator?.SetBool("isRunning", true);
        return Status.Running;
    }

    protected override void OnEnd()
    {
        animator?.SetBool("isRunning", false);
    }
}

