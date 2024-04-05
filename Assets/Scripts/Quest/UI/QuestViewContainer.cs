using CC.Event;
using UnityEngine;
using TMPro;

namespace CC.Quest.UI
{
    public class QuestViewContainer : MonoBehaviour
    {
        [SerializeField] int _index;
        [SerializeField] AQuest _questData;
        [SerializeField] SenderDataEventChannelSO _questPickEvent;
        [SerializeField] TextMeshProUGUI _text;
        public void assign(AQuest data, int ind)
        {
            _questData = data;
            _index = ind;
            _text.text = _questData.GetQuestName();
        }
        public void PickQuest()
        {
            _questPickEvent?.raiseEvent(this, _index);
        }
    }
}
