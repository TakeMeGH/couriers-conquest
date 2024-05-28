using AYellowpaper.SerializedCollections;
using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC.Items
{
    [CreateAssetMenu(fileName = "ListItem", menuName = "Items/List Item", order = 6)]
    public class ListItemID : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, ABaseItem> listItem;

        public ABaseItem GetItemByID(string key)
        {
            if (listItem.ContainsKey(key))
            {
                return listItem[key];
            }
            else
            {
                // Key tidak ditemukan, lakukan sesuatu seperti menampilkan pesan kesalahan atau mengembalikan null
                Debug.LogError("Item dengan kunci " + key + " tidak ditemukan!");
                return null;
            }
        }
    }
}
