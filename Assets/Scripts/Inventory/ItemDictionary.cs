using CC.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ItemDictionary
{
    private string _folderPath = "Assets/ScriptableObjects/Item/";
    private string[] _specificPath = { "Consumable", "Materials", "Quest", "Rune", "Equipment" }; 
    [SerializeField] private Dictionary<string, ABaseItem> _itemDictionary = new Dictionary<string, ABaseItem>();

    public void Initialize()
    {
        for(int i = 0; i < _specificPath.Length; i++)
        {
            AttachMaterials(_specificPath[i]);
        }
    }

    private void AttachMaterials(string path)
    {
        string[] assetGuids = AssetDatabase.FindAssets("t:" + typeof(ABaseItem).Name, new[] { _folderPath + path });

        // Menghapus isi list scriptableObjects sebelumnya
        _itemDictionary.Clear();

        foreach (string assetGuid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

            ABaseItem scriptableObject = AssetDatabase.LoadAssetAtPath<ABaseItem>(assetPath);

            if (scriptableObject != null)
            {
                _itemDictionary.Add(scriptableObject.name, scriptableObject);
                //Debug.Log("Add " + scriptableObject.name);
            }
        }
    }

    public ABaseItem GetValueByKey(string key)
    {
        if (_itemDictionary.ContainsKey(key))
        {
            return _itemDictionary[key];
        }
        else
        {
            Debug.LogWarning("Key tidak ditemukan dalam dictionary: " + key);
            return null;
        }
    }
}
