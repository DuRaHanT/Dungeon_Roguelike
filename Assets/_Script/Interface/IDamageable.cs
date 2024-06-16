public interface IDamageable
{
    int Health { get; set; }    
    int Attack { get; set; }

    void TakeDamage(int amount);
}
