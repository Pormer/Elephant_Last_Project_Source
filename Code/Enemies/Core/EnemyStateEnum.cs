using Unity.Behavior;

namespace Code.Enemies.Core
{
    [BlackboardEnum]
    public enum EnemyStateEnum
    {
        SLEEP = 0,
        WANDERING = 1,
        CHASE = 2,
        ATTACK = 4,
        HIT = 5,
        AIRBONE = 6,
        WAKEUP = 7,
        DEAD = 8,
    }
}