using UnityEngine;

namespace CC
{
    public class DeadPanelEnabler : MonoBehaviour
    {
        [SerializeField] GameObject _deadPanel;

        public void EnableDeadPanel()
        {
            _deadPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
