using UnityEngine;
using TMPro;
using System.Collections;

public class DisplayText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _fadeDuration = 2f;

    private Coroutine _fadeCoroutine;
    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(_fadeDuration);

        float startAlpha = _text.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;
            _text.alpha = Mathf.Lerp(startAlpha, 0f, t);

            yield return null;
        }

        _text.alpha = 0f;
    }

    private void SetText(string text)
    {
        _text.text = text;
        _text.alpha = 1f;

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeOutText());
    }

    public void TextShowActive(string gameObjectName)
    {
        _text.text = "База " + gameObjectName + " активирована для установки флага!";
        SetText(_text.text);
    }

    public void TextShowDeactivated(string gameObjectName)
    {
        _text.text = "База " + gameObjectName + " деактивирована!";
        SetText(_text.text);
    }

    public void UpdateClickPositionText(Vector3 position)
    {
        _text.text = "Позиция клика по земле: " + position;
        SetText(_text.text);
    }
}
