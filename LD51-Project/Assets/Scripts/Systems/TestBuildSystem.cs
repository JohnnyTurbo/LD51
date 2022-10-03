using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TMG.LD51
{
    [BurstCompile]
    [UpdateAfter(typeof(UnitSelectionSystem))]
    public partial struct TestBuildSystem : ISystem
    {
        private BuildState _buildState;
        private int _buildingIndex;
        
        public void OnCreate(ref SystemState state)
        {
            _buildState = BuildState.NotBuilding;
        }

        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            switch (_buildState)
            {
                case BuildState.NotBuilding:
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        _buildingIndex = 1;
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        _buildingIndex = 2;
                    }
                    
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        _buildingIndex = 3;
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        _buildingIndex = 4;
                    }
                    
                    var builder = SystemAPI.GetAspectRO<BuilderAspect>(SystemAPI.GetSingletonEntity<BuildingPrefabs>());
                    var buildingPrefab = GetBuildModel(builder);
                    if (buildingPrefab != Entity.Null) {
                        Debug.Log($"building {buildingPrefab.ToString()}");
                        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                            .CreateCommandBuffer(state.WorldUnmanaged);
                        
                        var newBuilding = ecb.Instantiate(buildingPrefab);
                        ecb.AddComponent<CurrentBuildingTag>(newBuilding);
                        _buildState = BuildState.PlacingBuilding;
                    }
                    break;
                
                case BuildState.PlacingBuilding:
                    var currentBuilding = SystemAPI.GetSingletonEntity<CurrentBuildingTag>();
                    Debug.Log($"current building is {currentBuilding.ToString()}");
                    var gridStuff = SystemAPI.GetSingleton<GridProperties>();
                    var curBuildingTransform = SystemAPI.GetAspectRW<TransformAspect>(currentBuilding);

                    var worldMousePos = state.World.GetExistingSystemManaged<UnitSelectionSystem>().WorldMousePosition;

                    var buildPosition = (math.floor(worldMousePos / gridStuff.UnitSize) * gridStuff.UnitSize) + gridStuff.Offset;
                    
                    curBuildingTransform.Position = buildPosition;
                    Debug.Log($"setting to {buildPosition}");
                    if (Input.GetMouseButtonDown(0))
                    {
                        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                            .CreateCommandBuffer(state.WorldUnmanaged);
                        var builder2 = SystemAPI.GetAspectRO<BuilderAspect>(SystemAPI.GetSingletonEntity<BuildingPrefabs>());
                        var finalPrefab = GetFinalModel(builder2);
                        var newBuilding = ecb.Instantiate(finalPrefab);
                        ecb.SetComponent(newBuilding, new Translation { Value = curBuildingTransform.Position });
                        ecb.SetComponent(newBuilding, new Rotation { Value = curBuildingTransform.Rotation });
                        if (SystemAPI.HasComponent<MinerProperties>(finalPrefab))
                        {
                            var baseMinerProperties = SystemAPI.GetComponent<MinerProperties>(finalPrefab);
                            
                            baseMinerProperties.SpawnPoint += curBuildingTransform.Position;
                            ecb.SetComponent(newBuilding, baseMinerProperties);
                        }
                        ecb.DestroyEntity(currentBuilding);
                        _buildState = BuildState.NotBuilding;
                        _buildingIndex = 0;
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        curBuildingTransform.RotateLocal(quaternion.RotateY(math.PI/2f));
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                            .CreateCommandBuffer(state.WorldUnmanaged);
                        ecb.DestroyEntity(currentBuilding);
                        _buildState = BuildState.NotBuilding;
                        _buildingIndex = 0;
                    }

                    break;
                case BuildState.PlacedBuilding:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private Entity GetBuildModel(BuilderAspect builder)
        {
            //var builder = SystemAPI.GetAspectRO<BuilderAspect>(SystemAPI.GetSingletonEntity<BuildingPrefabs>());
            return _buildingIndex switch
            {
                1 => builder.MinerBuild,
                2 => builder.ConveyorStraightBuild,
                3 => builder.ConveyorTurnCWBuild,
                4 => builder.ConveyorTurnCCWBuild,
                _ => Entity.Null
            };
        }
        
        private Entity GetFinalModel(BuilderAspect builder)
        {
            //var builder = SystemAPI.GetAspectRO<BuilderAspect>(SystemAPI.GetSingletonEntity<BuildingPrefabs>());
            return _buildingIndex switch
            {
                1 => builder.MinerFinal,
                2 => builder.ConveyorStraightFinal,
                3 => builder.ConveyorTurnCWFinal,
                4 => builder.ConveyorTurnCCWFinal,
                _ => Entity.Null
            };
        }
    }

    public struct BuildingState : IComponentData
    {
        public BuildState Value;
    }

    public enum BuildState
    {
        NotBuilding,
        PlacingBuilding,
        PlacedBuilding,
    }

    public struct CurrentBuildingTag : IComponentData {}
}