using System.Collections.Generic;
using UnityEngine;

namespace CrossRoundArena.Core
{
    public static class CombatSystem
    {
        public static bool CanAttack(MonsterInstance attacker, IBattleTarget target, out string reason)
        {
            reason = "";

            if (attacker == null)
            {
                reason = "Attacker is null.";
                return false;
            }

            if (attacker.hasAttackedThisTurn)
            {
                reason = "Already attacked this turn.";
                return false;
            }

            if (target == null)
            {
                reason = "Target is null.";
                return false;
            }

            if (target.IsDead)
            {
                reason = "Target is already dead.";
                return false;
            }

            // Target validation logic
            if (target is MonsterInstance targetMonster)
            {
                // If there's a Guard on the target's board, and this target is not a Guard, can't attack it.
                if (targetMonster.owner.HasGuardOnBoard() && !targetMonster.HasKeyword(Keyword.Guard))
                {
                    reason = "Must attack monsters with Guard first.";
                    return false;
                }
            }
            else if (target is PlayerState targetPlayer)
            {
                if (!targetPlayer.IsLeaderTargetable())
                {
                    reason = "Conditions to attack the leader are not met (Monsters > 4 or Guard exists).";
                    return false;
                }
            }

            return true;
        }

        public static void ExecuteAttack(MonsterInstance attacker, IBattleTarget target)
        {
            string reason;
            if (!CanAttack(attacker, target, out reason))
            {
                Debug.LogWarning($"Attack failed: {reason}");
                return;
            }

            attacker.Attack(target);
            CleanupDeadMonsters(attacker.owner);
            
            if (target is MonsterInstance targetMonster)
            {
                CleanupDeadMonsters(targetMonster.owner);
            }
        }

        private static void CleanupDeadMonsters(PlayerState player)
        {
            player.boardMonsters.RemoveAll(m => m.IsDead);
        }
    }
}
