using System.Collections;
using System.Collections.Generic;
using CC.Event;
using UnityEngine;

namespace CC
{
    public class LocationDetector : MonoBehaviour
    {
        [SerializeField] Location _choosenLocation;
        [SerializeField] SenderDataEventChannelSO _spawnLocationBanner;

        string[] _locationName = {
            "Luminare City",
            "SILVER FOREST",
            "Forsaken Land",
            "Little Valey Village"
            };


        private void OnTriggerEnter(Collider other)
        {
            switch (_choosenLocation)
            {
                case Location.LuminareCity:
                    AudioManager.instance.InitializeBGM(AudioManager.instance.CityBGM);
                    _spawnLocationBanner.raiseEvent(null, _locationName[0]);
                    break;
                case Location.SilverForest:
                    AudioManager.instance.InitializeBGM(AudioManager.instance.ForestBGM);
                    _spawnLocationBanner.raiseEvent(null, _locationName[1]);
                    break;
                case Location.ForsakenLand:
                    AudioManager.instance.InitializeBGM(AudioManager.instance.ForestBGM);
                    _spawnLocationBanner.raiseEvent(null, _locationName[2]);
                    break;
                case Location.LittleValeyVillage:
                    AudioManager.instance.InitializeBGM(AudioManager.instance.VillageBGM);
                    _spawnLocationBanner.raiseEvent(null, _locationName[3]);
                    break;
            }
        }
    }

    enum Location
    {
        LuminareCity,
        SilverForest,
        ForsakenLand,
        LittleValeyVillage,
    }
}
