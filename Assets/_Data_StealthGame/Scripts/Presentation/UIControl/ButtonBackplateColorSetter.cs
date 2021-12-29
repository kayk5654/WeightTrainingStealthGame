using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// set backplate color of buttons depending on the Interactable.IsEnabled
/// </summary>
public class ButtonBackplateColorSetter : MonoBehaviour
{
    [SerializeField, ColorUsage(true, true), Tooltip("enabled color")]
    private Color _enabledColor;

    [SerializeField, ColorUsage(true, true), Tooltip("disabled color")]
    private Color _disabledColor;

    [SerializeField, Tooltip("backplate renderer")]
    private MeshRenderer _backplateRenderer;

    // material of the backplate
    private Material _backplateMaterial;

    // primary color property
    private string _primaryColorProperty = "_PrimaryColor";

    // secondary color property
    private string _secondaryColorProperty = "_SecondaryColor";

    /// <summary>
    /// initialization
    /// </summary>
    private void Awake()
    {
        _backplateMaterial = _backplateRenderer.material;
    }

    /// <summary>
    /// update color
    /// </summary>
    private void Update()
    {
        
    }

    /// <summary>
    /// set backplate color
    /// </summary>
    public void SetColor(bool setEnabledColor)
    {
        // if the material is null, get reference
        if (!_backplateMaterial)
        {
            _backplateMaterial = _backplateRenderer.material;
        }
        
        if (setEnabledColor)
        {
            // set enabled color
            if(_backplateMaterial.GetColor(_primaryColorProperty) != _enabledColor)
            {
                _backplateMaterial.SetColor(_primaryColorProperty, _enabledColor);
                _backplateMaterial.SetColor(_secondaryColorProperty, _enabledColor);
            }
        }
        else
        {
            // set disabled color
            if (_backplateMaterial.GetColor(_primaryColorProperty) != _disabledColor)
            {
                _backplateMaterial.SetColor(_primaryColorProperty, _disabledColor);
                _backplateMaterial.SetColor(_secondaryColorProperty, _disabledColor);
            }
        }
    }
}
