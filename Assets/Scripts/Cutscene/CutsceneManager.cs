using System.Collections;
using System.Collections.Generic;
using CC.Event;
using UnityEngine;
using UnityEngine.Video;

namespace CC
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] float Cutscene_1;
        [SerializeField] float Cutscene_2;
        [SerializeField] float waitTime;
        [SerializeField] GameObject cutscene_1;
        [SerializeField] GameObject cutscene_2;
        [SerializeField] SenderDataEventChannelSO _onCutSceneFinished;
        [SerializeField] InputReader _inputReader;
        Coroutine playCutscene;
        private void Start()
        {
            cutscene_1.SetActive(true);
            cutscene_2.SetActive(false);

            _inputReader.EnableInventoryUIInput();

            AudioManager.instance.InitializeBGM(AudioManager.instance.Cutscene_1, true);
            playCutscene = StartCoroutine(PlayingCutscene());
        }

        IEnumerator PlayingCutscene()
        {
            yield return new WaitForSeconds(Cutscene_1);

            cutscene_1.SetActive(false);
            cutscene_2.SetActive(true);

            AudioManager.instance.InitializeBGM(AudioManager.instance.Cutscene_2, true);

            yield return new WaitForSeconds(Cutscene_2);

            OnCutSceneFinished();


        }
        // private void Update()
        // {
        //     Cutscene_1 -= Time.deltaTime;
        //     if (Cutscene_1 <= 0)
        //     {
        //         if (firstTime)
        //         {
        //             cutscene_1.SetActive(false);
        //             cutscene_2.SetActive(true);

        //             AudioManager.instance.InitializeBGM(AudioManager.instance.Cutscene_2, true);
        //             firstTime = false;
        //         }

        //         Cutscene_2 -= Time.deltaTime;
        //         if (Cutscene_2 <= 0)
        //         {
        //             if (!isFinish)
        //             {

        //             }
        //         }

        //     }
        // }

        public void OnCutSceneFinished()
        {
            if (playCutscene != null) StopCoroutine(playCutscene);

            cutscene_1.SetActive(false);
            cutscene_2.SetActive(false);

            AudioManager.instance.StopBGM(null, null);

            _onCutSceneFinished.raiseEvent(null, null);

        }

    }
}
