using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private SpriteLibrary library;
    [SerializeField] private SpriteResolver resolver;

    [SerializeField] private float swapInterval = 0.2f;

    private Coroutine swapAnimCoroutine;
    private string currentCategory = string.Empty;

    private void Awake()
    {
        if (library == null)
            library = GetComponentInChildren<SpriteLibrary>();
        
        if (resolver == null)
            resolver = GetComponentInChildren<SpriteResolver>();
    }

    public void Play(string categoryName, bool playLoop = false, float speedMultiplier = 1, Action onAnimationEnd = null)
    {
        if(currentCategory.Equals(categoryName)) return;

        currentCategory = categoryName;
        
        if(swapAnimCoroutine != null)
            StopCoroutine(swapAnimCoroutine);

        var labels = library.spriteLibraryAsset.GetCategoryLabelNames(categoryName).ToList();
        if (labels.Count == 0)
            throw new ArgumentException($"category {categoryName} not found", nameof(categoryName));
        
        int currentIndex = 0;

        swapAnimCoroutine = StartCoroutine(swapRoutine());
        
        IEnumerator swapRoutine()
        {
            while (categoryName.Equals(currentCategory) && (playLoop || currentIndex < labels.Count))
            {
                currentIndex %= labels.Count;
                
                resolver.SetCategoryAndLabel(categoryName, labels[currentIndex]);

                yield return new WaitForSeconds(swapInterval / speedMultiplier);

                if (!playLoop && currentIndex == labels.Count - 1)
                {
                    onAnimationEnd?.Invoke();
                    yield break;
                }
                
                currentIndex++;
            }
        }
    }
}
