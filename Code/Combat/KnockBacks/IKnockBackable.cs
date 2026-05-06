namespace Code.Combat.KnockBacks
{
    public interface IKnockBackable
    {
        public void KnockBack(KnockBackDataSO knockBackData, float xDirection);
        public bool CheckIsAirborne();
    }
}