using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TaskClasses
{
    public class Task : TaskBase
    {
        public class MoveToPosition : Task
        {
            public Vector3 targetPosition;

        }
        public class Victory : Task
        {

        }
        public class CleanUp : Task
        {
            public Vector3 targetPosition;
            public Action cleanUpAction;
        }
        public class TakeResourceToPosition : Task //grabs a resource and takes it to building? position
        {
            public Vector3 resourcePosition;
            public Action<TaskWorkerAI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }
        public class ConvertToTransporterTask : Task
        {
            public Vector3 buildingPosition;
            public Action<TaskWorkerAI> convertAction;
        }
    }
    public class TransporterTask : TaskBase
    {
        public class MoveToPosition : TransporterTask
        {
            public Vector3 targetPosition;
        }
        public class TakeWeaponFromSlotToPosition : TransporterTask
        {
            public Vector3 resourcePosition;
            public Action<Transporter_TaskWorkerAI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }
    }
    public class GhostTask : TaskBase
    {
        public class MoveToPosition : GhostTask
        {
            public Vector3 targetPosition;

        }

        public class FinishTutorial : GhostTask
        {
            public Vector3 targetPosition;
            public Action finishAction;
        }
    }
    public class Task_IEnemy_Unit : TaskBase
    {
        public class MoveToPosition : Task_IEnemy_Unit
        {
            public Vector3 targetPosition;

        }

        public class Victory : Task_IEnemy_Unit
        {

        }

        public class CleanUp : Task_IEnemy_Unit
        {
            public Vector3 targetPosition;
            public Action cleanUpAction;
        }

        public class TakeResourceToPosition : Task_IEnemy_Unit //grabs a resource and takes it to building? position
        {
            public Vector3 resourcePosition;
            public Action<Enemy_Spearman_AI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }

        public class ConvertToTransporterTask : Task_IEnemy_Unit
        {
            public Vector3 buildingPosition;
            public Action<Enemy_Spearman_AI> convertAction;
        }

        public class GetHitThenContinueToTarget : Task_IEnemy_Unit
        {
            public Action<Enemy_Spearman_AI> stopAction;
        }
    }
    public class TestTask : TaskBase
    {
        public class MoveToPosition : TestTask
        {
            public Vector3 targetPosition;

        }
        public class MoveToPositionThenDie : TestTask
        {
            public Vector3 targetPosition;
            public Action<TaskTestNewWorkerAI> DieAction;

        }
        public class StopAndAttack : TestTask
        {
            public Action<TaskTestNewWorkerAI> AttackAction;
        }
    }
    public class TestTaskHobo : TaskBase
    {
        public class MoveToPosition : TestTaskHobo
        {
            public Vector3 targetPosition;
        }
        public class ConvertToVillager : TestTaskHobo
        {
            public Vector3 targetPosition;
            public Action convertAction;

        }
    }
    public class TestTaskVillager : TaskBase
    {
        public class MoveToPosition : TestTaskVillager
        {
            public Vector3 targetPosition;
        }
        public class TakeResourceFromSlotToPosition : TestTaskVillager
        {
            public Vector3 resourcePosition;
            public Action<TaskTestVillagerAI> takeResource;
        }
        public class DropResourceFromPositionToSlot : TestTaskVillager
        {
            public Vector3 resourcePosition;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }
        public class ConvertToBuilder : TestTaskVillager
        {
            public Vector3 targetPosition;
            public Action<TaskTestVillagerAI> convertAction;

        }
        public class ConvertToArcher : TestTaskVillager
        {
            public Vector3 targetPosition;
            public Action<TaskTestVillagerAI> convertAction;
        }
    }
}
