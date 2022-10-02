using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace TMG.LD51
{
    public partial class UnitSelectionSystem : SystemBase
    {
        private Camera _mainCamera;
        private PhysicsWorldSingleton _physicsWorldSingleton;
        private CollisionWorld _collisionWorld;
        private CollisionFilter _unitsFilter;
        private CollisionFilter _groundFilter;
        
        protected override void OnStartRunning()
        {
            _mainCamera = Camera.main;

            _unitsFilter = new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Selection,
                CollidesWith = (uint)CollisionLayers.Units
            };
            
            _groundFilter = new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Selection,
                CollidesWith = (uint)CollisionLayers.Ground | (uint)CollisionLayers.Asteroids
            };
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectUnits();
                }
                
                SelectSingleUnit();
            }

            if (Input.GetMouseButtonUp(1))
            {
                SetTargetPosition();
            }
        }
        
        private void SelectSingleUnit()
        {
            _physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            
            if (RaycastToMousePosition(_unitsFilter, out var raycastHit))
            {
                var hitEntity = _physicsWorldSingleton.Bodies[raycastHit.RigidBodyIndex].Entity;
                if (EntityManager.HasComponent<SelectableUnitTag>(hitEntity))
                {
                    EntityManager.AddComponent<SelectedEntityTag>(hitEntity);
                }
            }
        }

        private void SetTargetPosition()
        {
            _physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            
            if (RaycastToMousePosition(_groundFilter, out var raycastHit))
            {
                var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                    .CreateCommandBuffer(World.Unmanaged);
                foreach (var drone in SystemAPI.Query<DroneAspect>().WithAll<SelectedEntityTag>())
                {
                    if (HasComponent<GroundTag>(raycastHit.Entity))
                    {
                        ecb.SetComponentEnabled<MoveToTargetTag>(drone.Entity, true);
                        ecb.SetComponentEnabled<HarvestAsteroidsTag>(drone.Entity, false);
                        drone.DroneState = DroneState.MovingToTargetPosition;
                        drone.TargetPosition = raycastHit.Position;
                    }
                    else if(HasComponent<AsteroidTag>(raycastHit.Entity))
                    {
                        ecb.SetComponentEnabled<MoveToTargetTag>(drone.Entity, false);
                        ecb.SetComponentEnabled<HarvestAsteroidsTag>(drone.Entity, true);
                        drone.AsteroidTarget = EntityManager.GetAspect<TransformAspect>(raycastHit.Entity).Position;
                    }
                }   
            }
        }

        private bool RaycastToMousePosition(CollisionFilter filter, out RaycastHit raycastHit)
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(1000f);

            return Raycast(rayStart, rayEnd, filter, out raycastHit);
        }

        private bool Raycast(float3 rayStart, float3 rayEnd, CollisionFilter filter, out RaycastHit raycastHit)
        {
            var raycastInput = new RaycastInput
            {
                Start = rayStart,
                End = rayEnd,
                Filter = filter
            };
            return _physicsWorldSingleton.CastRay(raycastInput, out raycastHit);
        }
        
        private void DeselectUnits()
        {
            EntityManager.RemoveComponent<SelectedEntityTag>(GetEntityQuery(typeof(SelectedEntityTag)));
        }
    }
    
    [Flags]
    public enum CollisionLayers
    {
        Selection = 1 << 0,
        Ground = 1 << 1,
        Units = 1 << 2,
        Asteroids = 1 << 3,
    }
}