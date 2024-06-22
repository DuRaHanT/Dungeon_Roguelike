using UnityEngine;
using UnityEngine.UI;

namespace Rog_Card
{
    public enum CardType
    {
        Attack,
        Buff,
        DeBuff,
        Defence,
        Move,
        Special
    }

    public class CardState : MonoBehaviour
    {
        public CardType cardType;
        public int statsData;
    }
}