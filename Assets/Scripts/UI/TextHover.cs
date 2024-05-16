using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CC.UI
{
    public class TextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI[] textMeshPro;
        public Color normalColor = Color.white;
        public Color hoverColor = Color.red;

        void Start()
        {
            textMeshPro = GetComponentsInChildren<TextMeshProUGUI>();

            for(int i = 0; i < textMeshPro.Length; i++)
            {
                textMeshPro[i].color = normalColor; // Ensure the text starts with the normal color
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            for (int i = 0; i < textMeshPro.Length; i++)
            {
                textMeshPro[i].color = hoverColor; // Ensure the text starts with the normal color
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            for (int i = 0; i < textMeshPro.Length; i++)
            {
                textMeshPro[i].color = normalColor; // Ensure the text starts with the normal color
            }
        }
    }
}
