using Code.Modules;
using UnityEngine;

namespace Code.Contexts.Summons
{
    public class SummonContext : IContext
    {
        public ModuleOwner Owner { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public SummonContext(ModuleOwner owner,  Vector3 position, Vector3 rotation)
        {
            Owner = owner;
            Position = position;
            Rotation = rotation;
        }
    }
}