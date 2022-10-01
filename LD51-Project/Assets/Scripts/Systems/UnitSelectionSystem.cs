using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace TMG.LD51
{
    public partial class UnitSelectionSystem : SystemBase
    {
        private Camera _mainCamera;
        private PhysicsWorld _physicsWorld;
        private CollisionWorld _collisionWorld;
        private CollisionFilter _unitsFilter;
        private CollisionFilter _groundFilter;
        
        protected override void OnStartRunning()
        {
            _mainCamera = Camera.main;
            _physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            _unitsFilter = new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Selection,
                CollidesWith = (uint)CollisionLayers.Units
            };
            
            _groundFilter = new CollisionFilter
            {
                BelongsTo = (uint)CollisionLayers.Selection,
                CollidesWith = (uint)CollisionLayers.Ground
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
            /*_collisionWorld = _physicsWorld.CollisionWorld;

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(1000f);

            if (Raycast(rayStart, rayEnd, out var raycastHit))
            {
                var hitEntity = _physicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
                if (EntityManager.HasComponent<SelectableUnitTag>(hitEntity))
                {
                    EntityManager.AddComponent<SelectedEntityTag>(hitEntity);
                }
            }*/
            if (RaycastToMousePosition(_unitsFilter, out var raycastHit))
            {
                var hitEntity = _physicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
                if (EntityManager.HasComponent<SelectableUnitTag>(hitEntity))
                {
                    EntityManager.AddComponent<SelectedEntityTag>(hitEntity);
                }
            }
        }

        private void SetTargetPosition()
        {
            Debug.Log("setting target posish");
            if (RaycastToMousePosition(_groundFilter, out var raycastHit))
            {
                Debug.Log("hit the ground");
                foreach (var drone in SystemAPI.Query<DroneAspect>().WithAll<SelectedEntityTag>())
                {
                    drone.TargetPosition = raycastHit.Position;
                    Debug.Log($"set the posish to {raycastHit.Position} on {drone.Self.Index}");
                }
            }
        }

        private bool RaycastToMousePosition(CollisionFilter filter, out RaycastHit raycastHit)
        {
            _collisionWorld = _physicsWorld.CollisionWorld;

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
            return _collisionWorld.CastRay(raycastInput, out raycastHit);
        }
        
        private void DeselectUnits()
        {
            EntityManager.RemoveComponent<SelectedEntityTag>(GetEntityQuery(typeof(SelectedEntityTag)));
            Debug.Log("removing");
        }
    }
    
    [Flags]
    public enum CollisionLayers
    {
        Selection = 1 << 0,
        Ground = 1 << 1,
        Units = 1 << 2
    }
}