using System;
using UnityEngine;
using CrossRoundArena.Data;
using UnityEngine.InputSystem;

namespace CrossRoundArena.Core
{
    public enum SelectionMode
    {
        None,
        SkillTarget,
        EquipmentTarget,
        AttackerSelect,
        AttackTarget
    }

    public class TargetSelectionManager : MonoBehaviour
    {
        public static TargetSelectionManager instance;

        public SelectionMode currentMode = SelectionMode.None;
        
        // Selection State
        private CardData pendingCard;
        private MonsterInstance pendingAttacker;
        private Action<IBattleTarget> onTargetSelected;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        public void StartSkillTargetSelection(SkillCardData card, Action<IBattleTarget> callback)
        {
            currentMode = SelectionMode.SkillTarget;
            pendingCard = card;
            onTargetSelected = callback;
            Debug.Log($"Select target for skill: {card.cardName}");
            // TODO: UI visual feedback for selection mode
        }

        public void StartEquipmentTargetSelection(EquipmentCardData card, Action<IBattleTarget> callback)
        {
            currentMode = SelectionMode.EquipmentTarget;
            pendingCard = card;
            onTargetSelected = callback;
            Debug.Log($"Select monster to equip: {card.cardName}");
        }

        public void StartAttackTargetSelection(MonsterInstance attacker, Action<IBattleTarget> callback)
        {
            currentMode = SelectionMode.AttackTarget;
            pendingAttacker = attacker;
            onTargetSelected = callback;
            Debug.Log($"Select target for attacker: {attacker.Data.cardName}");
        }

        public void OnTargetClicked(IBattleTarget target)
        {
            if (currentMode == SelectionMode.None) return;

            // Basic validation based on mode
            bool isValid = false;
            string failReason = "";

            switch (currentMode)
            {
                case SelectionMode.SkillTarget:
                    isValid = true; // Skill specific validation can be added
                    break;
                case SelectionMode.EquipmentTarget:
                    if (target is MonsterInstance) isValid = true;
                    else failReason = "Must select a monster for equipment.";
                    break;
                case SelectionMode.AttackTarget:
                    isValid = CombatSystem.CanAttack(pendingAttacker, target, out failReason);
                    break;
            }

            if (isValid)
            {
                var callback = onTargetSelected;
                CancelSelection();
                callback?.Invoke(target);
            }
            else
            {
                Debug.LogWarning($"Invalid target: {failReason}");
            }
        }

        public void CancelSelection()
        {
            currentMode = SelectionMode.None;
            pendingCard = null;
            pendingAttacker = null;
            onTargetSelected = null;
            Debug.Log("Selection cancelled.");
        }

        private void Update()
        {
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame) // New Input System
            {
                CancelSelection();
            }
        }
    }
}
