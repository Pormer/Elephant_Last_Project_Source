using Code.Modules;
using Code.Statuses;

namespace Code.Players.Statuses
{
    public class PlayerStunStatus : Status
    {
        private Player _player;
        
        public override void Initialize(ModuleOwner owner)
        {
            _player = owner as Player;
        }

        public override void Apply(float duration)
        {
            _player.ChangeState("STUN");
        }

        public override void End()
        {
            _player.ChangeState("IDLE");
        }
    }
}