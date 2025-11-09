using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Enchantment;
using System.Linq;

namespace Manager
{
    public class EnchantmentManager : MonoBehaviour
    {
        // Player Enchentment
        [Header("Main Weapon Enchantment")]
        public EnchantmentSO[] mainEnchantment = new EnchantmentSO[5];

        [Header("Sec Weapon Enchantment")]
        public EnchantmentSO[] secEnchantment = new EnchantmentSO[5];

        [SerializeField] private List<EnchantmentSO> lastUnlockEnchantments;
        [SerializeField] private List<GameObject> allEnchantmentsGameObject;
        
        [SerializeField] private GameObject EnchantmentContent;
        [SerializeField] private GameObject EnchantmentPrefab;

        private GameManager gameManager;
        private PlayerProgressManager playerProgressManager;

        //float num = 0;

        // Enemy Enchantment (if I can do)

        void Start()
        {
            gameManager = GameManager.Instance.GetComponent<GameManager>();
            playerProgressManager = gameManager.playerProgressManager;

            if (!gameManager || !playerProgressManager)
            {
                Debug.LogError("Error! Game Manager or playerProguessManager.CS not found!");
            }

            foreach (EnchantmentSO enchantment in playerProgressManager.preUpadateTestUnlockEnchantment)
            {
                GameObject instance = GameObject.Instantiate(EnchantmentPrefab);
                instance.name = enchantment.name;
                instance.GetComponent<EnchantmentDesc>().enchantmentSO = enchantment;
                instance.transform.SetParent(EnchantmentContent.transform);
                allEnchantmentsGameObject.Add(instance);
                lastUnlockEnchantments.Add(enchantment);
            }


        }

        // Update is called once per frame
        void Update()
        {
            
            /*if (num > 1)
            {
                Debug.Log("Updating Enchantment");
                TestUpdateUnlockEnchantment();
                UpdateEnchantmentContent(playerProgressManager.NewUnlockEnchantmentList);

                num = -1;
            } else if (num != -1)
            {
                num += Time.deltaTime;
            }*/
        }



        private void TestUpdateUnlockEnchantment()
        {
            playerProgressManager.unlockedEnchantments.Clear();

            foreach(var enchantment in playerProgressManager.NewUnlockEnchantmentList)
            {
                playerProgressManager.unlockedEnchantments.Add(enchantment);
            }
        }

        public void UpdateEnchantmentContent(List<EnchantmentSO> newUnlockEnchantment)
        {
            var current = playerProgressManager.GetUnlockEnchantment();
            if (current.SequenceEqual(lastUnlockEnchantments)) return;

            // Compute delta / find difference
            var added = current.Except(lastUnlockEnchantments).ToList();
            var removed = lastUnlockEnchantments.Except(current).ToList();

            

            foreach (var enchant in removed)
            {
                //Debug.Log($"Disable{enchant.name}");
                DisableGameObject(enchant.name);
            }

            foreach (var enchant in added)
            {
                //Debug.Log($"Enable{enchant.name}");
                CreateOrUpdateSlot(enchant);
            }

            lastUnlockEnchantments = current.ToList();
        }

        private void CreateOrUpdateSlot(EnchantmentSO enchantment)
        {
            /*foreach (var n in allEnchantmentsGameObject)
            {
                if (n.name == name)
                {
                    n.gameObject.SetActive(true);
                    return;
                }
            }
*/
            GameObject existing = allEnchantmentsGameObject.FirstOrDefault(go => go.name == enchantment.name);
            if (existing == null)
            {
                // Instantiate new
                GameObject instance = Instantiate(EnchantmentPrefab, EnchantmentContent.transform);
                instance.name = enchantment.name;
                instance.GetComponent<EnchantmentDesc>().enchantmentSO = enchantment;
                allEnchantmentsGameObject.Add(instance);
            }
            else
            {
                // Enable existing
                existing.SetActive(true);
            }
        }

        private void DisableGameObject(string name)
        {
            /*foreach (var n in allEnchantmentsGameObject)
            {
                if (n.name == name)
                {
                    n.gameObject.SetActive(false);
                    return;
                }
            }*/

            GameObject slot = allEnchantmentsGameObject.FirstOrDefault(go => go.name == name);
            if (slot != null)
            {
                slot.SetActive(false);
            }
        }

        private void EnchantmentDebug(List<EnchantmentSO> added, List<EnchantmentSO> removed )
        {
            Debug.Log($"There are {added.Count} add enchantment");
            foreach (var item in added)
            {
                Debug.Log($"Add {item.name}");
            }

            Debug.Log($"There are {removed.Count} remove enchantment");
            foreach (var item in removed)
            {
                Debug.Log($"Remove {item.name}");
            }
        }
    }
}