using UnityEngine;
using System.Collections.Generic;

namespace CC.Inventory
{
    public class ItemDictionary : MonoBehaviour
    {
        [SerializeField] private string[] _specificPath = { "Consumable", "Materials", "Quest", "Rune", "Equipment" };
        private Dictionary<string, ABaseItem> _itemDictionary = new Dictionary<string, ABaseItem>();

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            foreach (string path in _specificPath)
            {
                AttachMaterials(path);
            }
        }

        private void AttachMaterials(string path)
        {
            ABaseItem[] items = Resources.LoadAll<ABaseItem>("Item/" + path);
            _itemDictionary.Clear(); // Consider clearing the dictionary outside this loop if you do not intend to reset it each time

            foreach (ABaseItem item in items)
            {
                if (item != null)
                {
                    _itemDictionary.Add(item.name, item);
                }
            }
        }

        public ABaseItem GetValueByKey(string key)
        {
            ABaseItem item;
            if (_itemDictionary.TryGetValue(key, out item))
            {
                return item;
            }
            else
            {
                Debug.LogWarning("Key not found in dictionary: " + key);
                return null;
            }
        }
    }

}
