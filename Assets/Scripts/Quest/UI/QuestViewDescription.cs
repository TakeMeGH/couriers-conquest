using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CC.Quest.UI
{
    public class QuestViewDescription : MonoBehaviour
    {
        [SerializeField] AQuest _quest;
        [SerializeField] TextMeshProUGUI _desc;
        public void assign(AQuest quest)
        {
            _quest = quest;
            refreshText();
        }
        public void refreshText()
        {
            _desc.text = _quest.GetQuestDescription();
        }
    }
}
