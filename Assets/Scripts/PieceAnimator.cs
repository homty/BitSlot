using System;
using System.Collections;
using UnityEngine;

public class PieceAnimator : MonoBehaviour
{
    public static PieceAnimator Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public IEnumerator AnimateMatchEffect(GameObject piece, float delay, Action<GameObject> onBeforeDestroy = null)
    {
        if (piece == null || piece.Equals(null)) yield break;

        yield return StartCoroutine(AnimateScaleBounce(piece));
        yield return StartCoroutine(AnimateShake(piece));

        onBeforeDestroy?.Invoke(piece);

        yield return new WaitForSeconds(delay);
    }

    public IEnumerator AnimateScaleBounce(GameObject piece)
    {
        if (piece == null || piece.Equals(null)) yield break;

        Vector3 originalScale = piece.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            if (piece == null || piece.Equals(null)) yield break;
            piece.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        piece.transform.localScale = targetScale;
    }

    public IEnumerator AnimateShake(GameObject piece)
    {
        if (piece == null || piece.Equals(null)) yield break;

        float shakeDuration = 0.3f;
        float shakeMagnitude = 0.05f;
        float elapsedTime = 0f;
        Vector3 originalPos = piece.transform.position;

        while (elapsedTime < shakeDuration)
        {
            if (piece == null || piece.Equals(null)) yield break;

            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            piece.transform.position = originalPos + randomOffset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (piece != null && !piece.Equals(null))
            piece.transform.position = originalPos;
    }

    public IEnumerator AnimateAppearance(GameObject piece)
    {
        if (piece == null || piece.Equals(null)) yield break;

        Vector3 targetScale = piece.transform.localScale;
        piece.transform.localScale = Vector3.zero;

        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            if (piece == null || piece.Equals(null)) yield break;

            piece.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        piece.transform.localScale = targetScale;
    }
}
