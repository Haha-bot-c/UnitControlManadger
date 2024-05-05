using UnityEngine;
using TMPro;
using DG.Tweening;

public class BaseTextInfo : MonoBehaviour
{
    private const float MaxAlpha = 1f;
    private const float MinAlpha = 0f;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _fadeDuration = 3.5f;

    private void SetText(string text)
    {
        FadeOutText(); 
        _text.text = text; 
    }

    private void FadeOutText()
    {
        _text.DOFade(MinAlpha, _fadeDuration).From(MaxAlpha); 
    }

    public void TextShowActive(string gameObjectName)
    {
        SetText("База " + gameObjectName + " активирована для установки флага!");
    }

    public void TextShowDeactivated(string gameObjectName)
    {
        SetText("База " + gameObjectName + " деактивирована!");
    }

    public void UpdateClickPositionText(Vector3 position)
    {
        SetText("Позиция клика по земле: " + position);
    }
}
