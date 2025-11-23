using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using Stats;
using Manager;
using System;

namespace Ui
{
    public class PlayerHPUi : MonoBehaviour
    {
        [SerializeField] SpriteRenderer[] playerHpSpriteSlot;
        [SerializeField] Sprite playerHpSprite;
        [SerializeField] Sprite playerHpEmptySprite;

        private PlayerStat playerStat;
        [SerializeField] private int curUiHP;
        public int maxUiHP;
        void Start()
        {
            playerStat = GameManager.Instance.player.GetComponent<PlayerStat>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateHPUi()
        {
            if (playerStat.curHp == curUiHP) return;

            int curUiHPindex = curUiHP--;
            int curHPindex = Mathf.FloorToInt(playerStat.curHp--);
            int diffIndex = curUiHPindex - curHPindex;
            if (diffIndex == 0) return;

            if (diffIndex > 0 && curUiHP < maxUiHP)
            {
                do
                {
                    curHPindex++;
                    playerHpSpriteSlot[curHPindex].sprite = playerHpSprite;
                    diffIndex--;
                } while (diffIndex > 0 || curUiHP < maxUiHP);
            }
            else if (diffIndex < 0 && curUiHP > 0)
            {
                do
                {
                    diffIndex = -diffIndex;
                    curHPindex--;
                    playerHpSpriteSlot[curHPindex].sprite = playerHpSprite;
                    diffIndex--;
                } while (diffIndex > 0 || curUiHP > 0);
            }
        }
    }
}