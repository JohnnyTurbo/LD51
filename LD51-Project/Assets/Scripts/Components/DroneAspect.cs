using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD51
{
    public readonly partial struct DroneAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly TransformAspect _transform;
        private readonly RefRW<TargetPosition> _targetPosition;
        private readonly RefRW<TargetAsteroidPosition> _asteroidTarget;
        private readonly RefRW<HomeBasePosition> _homeBasePosition;
        private readonly RefRW<CurrentDroneState> _droneState;
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
            get => _targetPosition.ValueRO.Value;
            set => _targetPosition.ValueRW.Value = value;
        }

        public quaternion Rotation
        {
            get => _transform.Rotation;
            set => _transform.Rotation = value;
        }

        public float3 AsteroidTarget
        {
            get => _asteroidTarget.ValueRO.Value;
            set => _asteroidTarget.ValueRW.Value = value;
        }
        
        public float3 HomeBasePosition
        {
            get => _homeBasePosition.ValueRO.Value;
            set => _homeBasePosition.ValueRW.Value = value;
        }

        public DroneState DroneState
        {
            get => _droneState.ValueRO.Value;
            set => _droneState.ValueRW.Value = value;
        }
        
        public float3 Forward => _transform.Forward;
        
        public float3 MoveSpeed => _moveSpeed.ValueRO.Value;

        public float RotationSpeed => _rotationSpeed.ValueRO.Value;

        public bool ShouldMoveToTarget => math.distancesq(Position, TargetPosition) > DIST_SQ;

        public bool IsFull => false;

        public void RotateTowardsHomeBase(float deltaTime)
        {
            var rotationTarget = MathUtilities.GetRotationToPoint(Position, HomeBasePosition);
        }

        public void MoveToTargetPosition(float deltaTime)
        {
            var rotationTarget = MathUtilities.GetRotationToPoint(Position, TargetPosition);
            RotateTowards(rotationTarget, deltaTime);
            if (math.distancesq(Position, TargetPosition) > DIST_SQ)
            {
                Position += Forward * MoveSpeed * deltaTime;
            }
            else
            {
                DroneState = DroneState.Stopped;
            }
        }
        
        public void MoveToAsteroid(float deltaTime)
        {
            var rotationTarget = MathUtilities.GetRotationToPoint(Position, AsteroidTarget);
            RotateTowards(rotationTarget, deltaTime);
            if (math.distancesq(Position, AsteroidTarget) > DIST_SQ)
            {
                Position += Forward * MoveSpeed * deltaTime;
            }
            else
            {
                DroneState = DroneState.Harvesting;
            }
        }

        public void MoveToHomeBase(float deltaTime)
        {
            var rotationTarget = MathUtilities.GetRotationToPoint(Position, HomeBasePosition);
            RotateTowards(rotationTarget, deltaTime);
            if (math.distancesq(Position, HomeBasePosition) > DIST_SQ)
            {
                Position += Forward * MoveSpeed * deltaTime;
            }
            else
            {
                DroneState = DroneState.Unloading;
            }
        }

        private void RotateTowards(quaternion rotationTarget, float deltaTime)
        {
            Rotation = MathUtilities.SetRotationTowards(Rotation, rotationTarget, RotationSpeed * deltaTime);
        }
    }
}