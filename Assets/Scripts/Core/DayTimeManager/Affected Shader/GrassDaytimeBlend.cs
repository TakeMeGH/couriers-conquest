using CC.Core.Daytime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CC
{
    public class GrassDaytimeBlend : MonoBehaviour
    {
        [SerializeField] DayTimeDataModel _model;
        float _currentTimeGrad;
        private void Update()
        {
            _currentTimeGrad = _model.getTime() / 1440f;

        }
    }
}
