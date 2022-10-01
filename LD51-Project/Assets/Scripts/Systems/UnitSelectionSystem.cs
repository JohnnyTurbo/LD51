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
        
        protected override void OnStartRunning()
        {
            _mainCamera = Camera.main;
            _physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
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
        }
        
        private void SelectSingleUnit()
        {
            Debug.Log("Selecting Single Unit");
            _collisionWorld = _physicsWorld.CollisionWorld;

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayStart = ray.origin;
            var rayEnd = ray.GetPoint(1000f);

            if (Raycast(rayStart, rayEnd, out var raycastHit))
            {
                var hitEntity = _physicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
                Debug.Log($"Raycast hit! {hitEntity.ToString()}");
                if (EntityManager.HasComponent<SelectableUnitTag>(hitEntity))
                {
                    EntityManager.AddComponent<SelectedEntityTag>(hitEntity);
                    Debug.Log("adding component");
                }
            }
            else
            {
                Debug.Log("No hiit");
            }
        }

        private bool Raycast(float3 rayStart, float3 rayEnd, out RaycastHit raycastHit)
        {
            var raycastInput = new RaycastInput
            {
                Start = rayStart,
                End = rayEnd,
                Filter = new CollisionFilter
                {
                    BelongsTo = (uint) CollisionLayers.Selection,
                    CollidesWith = (uint) (CollisionLayers.Ground | CollisionLayers.Units)
                }
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