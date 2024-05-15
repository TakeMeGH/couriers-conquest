using System.Collections;
using System.Collections.Generic;
using CC.Characters;
using CC.Core.Data.Dynamic;
using CC.Event;
using UnityEngine;

namespace CC
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] SenderDataEventChannelSO _activateQuestEvent;
        [SerializeField] PlayerStateSO _playerStateData;
        [SerializeField] int _prologueQuestId;

        private void Start()
        {
            if (!_playerStateData.IsFinishedQuest(_prologueQuestId))
            {
                _activateQuestEvent.raiseEvent(this, _prologueQuestId);
            }
        }
    }
}
