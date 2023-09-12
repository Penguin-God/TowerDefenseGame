using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichGetRicherController : StageUpGoldRewardCalculator
{
    const int GoldForInterest = 10;
    IBattleCurrencyManager _currencyManager;

    public RichGetRicherController(int rewradGold, IBattleCurrencyManager currencyManager) : base(rewradGold)
        => _currencyManager = currencyManager;

    public override int CalculateRewradGold() => base.CalculateRewradGold() + CalculateInterest();

    int CalculateInterest()
    {
        //int interestApplicableGold = Mathf.Min(_currencyManager.Gold, maxInterestGold);
        //return interestApplicableGold / GoldForInterest * interestGoldRate;
        return 0;
    }
}
