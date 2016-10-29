﻿using ColossalFramework;
using DaylightClassic.OptionsFramework;
using UnityEngine;

namespace DaylightClassic
{
    public class DayNightCycleMonitor : MonoBehaviour
    {
        private bool _previousEffectState;
        private bool _previousFogColorState;
        private bool _initialized;

        public void Awake()
        {
            var dayNightEnabled = Singleton<SimulationManager>.instance.m_enableDayNight;
            SetUpEffects(dayNightEnabled);
        }

        public void Update()
        {
            var dayNightEnabled = Singleton<SimulationManager>.instance.m_enableDayNight;
            if (dayNightEnabled == _previousEffectState && _previousFogColorState == OptionsWrapper<Options>.Options.fogColor && _initialized)
            {
                return;
            }
            SetUpEffects(dayNightEnabled);
            _initialized = true;
        }

        public void OnDestroy()
        {
            SetUpEffects(true);
        }

        private void SetUpEffects(bool dayNightEnabled)
        {
            var behaviors = Camera.main.GetComponents<MonoBehaviour>();
            foreach (var behavior in behaviors)
            {
                if (behavior is FogEffect)
                {
                    behavior.enabled = !dayNightEnabled;
                    if (behavior.enabled)
                    {
                        DaylightClassic.ReplaceFogColorImpl(false);
                    }
                }
                if (behavior is DayNightFogEffect)
                {
                    behavior.enabled = dayNightEnabled;
                    if (behavior.enabled)
                    {
                        DaylightClassic.ReplaceFogColorImpl(OptionsWrapper<Options>.Options.fogColor);                        
                    }
                }
            }
            _previousEffectState = dayNightEnabled;
            _previousFogColorState = OptionsWrapper<Options>.Options.fogColor;
        }
    }
}