public interface IDamageable
{
    int Health { get; set; }    
    int Attack { get; set; }
    int Armor { get; set; }

    void TakeDamage(int amount);
}
