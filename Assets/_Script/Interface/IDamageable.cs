using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IDamageable
{
    int Health { get; set;}    

    void TakeDamge(int amount);
}
