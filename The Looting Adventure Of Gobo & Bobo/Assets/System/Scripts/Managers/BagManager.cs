using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagManager : Singleton<BagManager>
{
    [HideInInspector]
    public bool CanTakeLoot = true;
    [HideInInspector]
    public bool HaveLoot = false;

    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenGivingLootToVendor;

    [Header("References")]
    [SerializeField]
    private Image bagFillImage = null;

    [Header("Variables")]
    [SerializeField]
    public int MaxNumberOfLootContainedInGoblinBag = 5;

    //helpers
    private List<Loot> lootInBag = new List<Loot>();
    private int currentNumberOfLootInBag = 0;
    private float targetFillAmount = 0f;
    private float initialFillAmount = 0f;
    private int bonusLootGrabbed = 0;
    private int amountToReduce = 0;
    private int totalLootScore = 0;
    private void OnDestroy()
    {
        _instance = null;
    }
    public bool CanTakeMoreLoot()
    {
        return currentNumberOfLootInBag < MaxNumberOfLootContainedInGoblinBag;
    }
    public void AddLootToBag(Loot loot)
    {
        lootInBag.Add(loot);
        SoundManager.Instance.PlayTakeLootSFX();
        currentNumberOfLootInBag++;
        HaveLoot = true;
        if(currentNumberOfLootInBag == MaxNumberOfLootContainedInGoblinBag)
        {
            CanTakeLoot = false;
        }

        targetFillAmount = 1f / MaxNumberOfLootContainedInGoblinBag * currentNumberOfLootInBag;
        initialFillAmount = bagFillImage.fillAmount;
        StartCoroutine(TweenFillImage());
    }
    public void GiveLootToVendor()
    {
        SoundManager.Instance.PlayGiveLootSFX();
        targetFillAmount = 0f;
        initialFillAmount = bagFillImage.fillAmount;
        StartCoroutine(TweenFillImage());
        foreach (Loot loot in lootInBag)
        {
            totalLootScore += (loot.BonusLoot) ? loot.LootScore + (750 * (1 * bonusLootGrabbed)) : loot.LootScore;
            if (loot.BonusLoot)
            {
                bonusLootGrabbed++;
            }
        }
        totalLootScore -= amountToReduce;
        ScoreManager.Instance.AddScore(totalLootScore);
        EventManager.Instance.InvokeTimedEvents(whenGivingLootToVendor);

        lootInBag.Clear();
        currentNumberOfLootInBag = 0;
        HaveLoot = false;
        CanTakeLoot = true;
    }
    public void AddToAmountToReduce()
    {
        if (HaveLoot)
        {
            amountToReduce++;
            Debug.Log(amountToReduce);
        }
    }
    IEnumerator TweenFillImage(float fillValue = 0f)
    {
        if(fillValue < 1f)
        {
            fillValue += Time.deltaTime;
            bagFillImage.fillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, fillValue);
            yield return null;
            StartCoroutine(TweenFillImage(fillValue));
        }
    }
}
