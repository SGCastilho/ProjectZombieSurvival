namespace Core.Interfaces
{
    public interface IDamagable
    {
        void DoDamage(int amount);
        void Recovery(int amount);
    }
}
