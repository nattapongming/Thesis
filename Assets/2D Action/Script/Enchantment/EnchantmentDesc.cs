using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Enchantment
{
    public class EnchantmentDesc : MonoBehaviour
    {
        public EnchantmentSO enchantmentSO;


        [SerializeField] Image enchantmentSprite;
        [SerializeField] TMP_Text enchantmentName;
        [SerializeField] TMP_Text enchantmentEffectDesc1;
        [SerializeField] TMP_Text enchantmentEffectDesc2;
        [SerializeField] TMP_Text enchantmentEffectDesc3;



        // Start is called before the first frame update
        void Start()
        {
            //enchantment = enchantmentSO.GetComponent<Enchantment>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateUi(EnchantmentSO enchantment = null)
        {
            if (enchantment)
            {
                enchantmentSprite.sprite = enchantmentSO.enchantmentSprite;

                enchantmentName.text = enchantmentSO.enchantmentName;
            }
            else
            {
                enchantmentSprite = null;

                enchantmentName = null;
                
            }

            enchantmentName.text = enchantmentSO.enchantmentName;
            /*enchantmentEffectDesc1.text = enchantment.enchantmentDesc1;
            enchantmentEffectDesc2.text = enchantment.enchantmentDesc2;
            enchantmentEffectDesc3.text = enchantment.enchantmentDesc3;*/

        }

        public void UpdateDesc()
        {

        }
    }
}