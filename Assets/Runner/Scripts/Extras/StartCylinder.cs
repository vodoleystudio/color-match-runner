using HyperCasual.Core;
using HyperCasual.Gameplay;
using HyperCasual.Runner;
using System;
using System.Collections;
using UnityEngine;

public class StartCylinder : MonoBehaviour
{
    [SerializeField]
    private GenericGameEventListener m_HudEnableEvent;

    private string m_FirstLevelDefinitionName => LevelManager.Instance.LevelDefinition.name;

    private Hud m_Hud => UIManager.Instance.GetView<Hud>();

    private const string k_FirstLevelName = "Level_1";

    private void OnEnable()
    {
        m_HudEnableEvent?.Subscribe();
    }

    private void OnDisable()
    {
        m_HudEnableEvent?.Unsubscribe();
    }

    private void Start()
    {
        if (m_HudEnableEvent != null)
        {
            m_HudEnableEvent.EventHandler = OnHudEnable;
        }
    }

    private void OnHudEnable()
    {
        if (m_FirstLevelDefinitionName == k_FirstLevelName)
        {
            m_Hud.ShowTutorial();
        }
    }
}