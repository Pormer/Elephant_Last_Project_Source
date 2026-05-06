using Code.Combat;
using Code.Entities;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Players
{
    [DefaultExecutionOrder(-1)]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField, Inject] private Player player;
        [SerializeField] private EntityFinderSO playerFinder;
        [SerializeField] private EntityCombatDataSO combatData;
        
        private void Awake()
        {
            playerFinder.SetTarget(player);
            player.SetCombatData(combatData);
        }
    }
}