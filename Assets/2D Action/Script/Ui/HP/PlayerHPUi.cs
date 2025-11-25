using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using Stats;
using Manager;
using System;
using UnityEngine.UI;

namespace Ui
{
    public class PlayerHPUi : MonoBehaviour
    {
        [SerializeField] Image[] playerHpSpriteSlot;
        [SerializeField] Sprite playerHpSprite;
        [SerializeField] Sprite playerHpEmptySprite;

        private PlayerStat playerStat;
        [SerializeField] private int curUiHP;
        public int maxUiHP;
        void Start()
        {
            playerStat = GameManager.Instance.player.GetComponent<PlayerStat>();
            maxUiHP = Mathf.FloorToInt(playerStat.maxHp);  
            curUiHP = maxUiHP;                             
            UpdateAllSlots();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateHPUi()
        {
            int targetUiHP = Mathf.FloorToInt(playerStat.curHp);
            if (targetUiHP == curUiHP) return;

            // Demage empty the slot
            if (targetUiHP < curUiHP)
            {
                for (int i = curUiHP - 1; i >= targetUiHP; i--)
                {
                    playerHpSpriteSlot[i].sprite = playerHpEmptySprite;
                }
            }

            // Heal fill the slot
            if (targetUiHP > curUiHP)
            {
                for (int i = curUiHP; i < targetUiHP; i++)
                {
                    playerHpSpriteSlot[i].sprite = playerHpSprite;
                }
            }
            
        }

        private void UpdateAllSlots()
        {
            for (int i = 0; i < playerHpSpriteSlot.Length; i++)
            {
                if (i < curUiHP)
                    playerHpSpriteSlot[i].sprite = playerHpSprite;
                else
                    playerHpSpriteSlot[i].sprite = playerHpEmptySprite;
            }
        }
    }
}