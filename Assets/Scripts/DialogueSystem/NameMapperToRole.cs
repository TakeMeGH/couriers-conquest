using UnityEngine;
using TMPro;

namespace CC.Dialogue
{
    public class NameMapperToRole : MonoBehaviour
    {
        public AYellowpaper.SerializedCollections.SerializedDictionary<string, string> NameToRole;
        [SerializeField] TMP_Text _characterName;
        [SerializeField] TMP_Text _roleText;
        void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        }

        void OnTextChanged(Object obj)
        {
            if (obj == _characterName)
            {
                string name = _characterName.text;
                name = name.Trim();
                if (NameToRole.ContainsKey(name))
                {
                    _roleText.text = NameToRole[name];
                }
                else
                {
                    _roleText.text = "";
                }
                _roleText.ForceMeshUpdate(true);
            }
        }
    }
}
