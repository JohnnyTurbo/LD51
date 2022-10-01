using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD51
{
    readonly partial struct DroneAspect : IAspect
    {
        public readonly Entity Self;
        
        private readonly TransformAspect _transform;
        private readonly RefRW<TargetPosition> _targetPosition;
        private readonly RefRO<MoveSpeed> _moveSpeed;
        private readonly RefRO<RotationSpeed> _rotationSpeed;

        private const float DIST_SQ = 25f;
        
        public float3 Position
        {
            get => _transform.Position;
            set => _transform.Position = value;
        }

        public float3 TargetPosition
        {
            get => _targetPosition.ValueRW.Value;
            set => _targetPosition.ValueRW.Value = value;
        }

        public quaternion Rotation
        {
            get => _transform.Rotation;
            set => _transform.Rotation = value;
        }
        
        public float3 Forward => _transform.Forward;
        
        public float3 MoveSpeed => _moveSpeed.ValueRO.Value;

        public float RotationSpeed => _rotationSpeed.ValueRO.Value;

        public bool ShouldMoveToTarget => math.distancesq(Position, TargetPosition) > DIST_SQ;

        public void RotateTowardsTarget(float deltaTime)
        {
            var rotationTarget = MathUtilities.GetRotationToPoint(Position, TargetPosition);
            Rotation = MathUtilities.SetRotationTowards(Rotation, rotationTarget, RotationSpeed * deltaTime);
        }
    }
}