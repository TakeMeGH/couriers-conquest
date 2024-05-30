using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CC.Quest.UI
{
    public class QuestViewDescription : MonoBehaviour
    {
        [SerializeField] AQuest _quest;
        [Header("Visuals")]
        [SerializeField] TextMeshProUGUI _desc;
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _rangeFrom;
        [SerializeField] Transform _rewardLayout;
        [SerializeField] GameObject _startButton;
        [SerializeField] GameObject _cancelButton;
        [Header("Prefab")]
        [SerializeField] GameObject _rewardUI;
        [SerializeField] Sprite _gold;
        [SerializeField] Sprite _exp;
        public void assign(AQuest quest)
        {
            _quest = quest;
            refreshText();
        }
        public void refreshText()
        {
            _desc.text = _quest.GetQuestDescription();
            _title.text = _quest.GetQuestName();
            _rangeFrom.text = (_quest.GetQuestArea() == null || _quest.GetQuestArea() == "") ? "Unknown":_quest.GetQuestArea();
            populateRewardInfo();
            identifyQuest();
        }

        public void populateRewardInfo()
        {
            foreach (Transform child in _rewardLayout.transform)
            {
                Destroy(child.gameObject);
            }
            Reward _reward = _quest.GetReward();

            var temp = Instantiate(_rewardUI, _rewardLayout);
            temp.GetComponent<ItemRewardUIHandler>().set(_gold, _reward.gold);
            temp = Instantiate(_rewardUI, _rewardLayout);
            temp.GetComponent<ItemRewardUIHandler>().set(_exp, _reward.exp);

            foreach(var item in _reward.item.Keys)
            {
                temp = Instantiate(_rewardUI, _rewardLayout);
                temp.GetComponent<ItemRewardUIHandler>().set(item.itemSprite, _reward.item[item]);
            }
        }

        public void identifyQuest()
        {
            bool _isAccepted = FindObjectOfType<QuestManager>().GetAcceptedQuest() == _quest;
            _startButton.SetActive(!_isAccepted);
            _cancelButton.SetActive(_isAccepted);
        }
    }
}
