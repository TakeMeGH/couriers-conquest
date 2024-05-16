using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CC.Inventory
{
    public class UIPanelShop : MonoBehaviour
    {
        // Start is called before the first frame update
        private AItemPanel _panelItem;
        private ABaseItem _item;

        [Space]
        [SerializeField] private TextMeshProUGUI _textItemName;
        [SerializeField] private TextMeshProUGUI _textHarga;

        public void Initialize(ABaseItem item)
        {
            _item = item;

            _textItemName.text = item.itemName;
            _textHarga.text = item.costSell.ToString();
        }

        private void OnEnable()
        {
            _panelItem = GetComponentInChildren<AItemPanel>();
        }

    }
}
