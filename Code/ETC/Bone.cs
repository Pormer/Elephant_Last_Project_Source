using Code.Contexts.Projectiles;
using Code.Summons.Projectiles;

namespace Code.ETC
{
    public class Bone : Projectile<ProjectileContext>
    {
        private void FixedUpdate()
        {
            _rigid.linearVelocity = _dir * _speed;
        }
    }
}