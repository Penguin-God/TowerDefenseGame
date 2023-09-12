using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichGetRicherController : StageUpGoldRewardCalculator
{
    const int GoldForInterest = 10;
    readonly IBattleCurrencyManager _currencyManager;
    readonly int _interestGoldRate;
    readonly int _maxInterestGold;
    public RichGetRicherController(int rewradGold, int interestGoldRate, int maxInterestGold,  IBattleCurrencyManager currencyManager) : base(rewradGold)
    {
        _currencyManager = currencyManager;
        _interestGoldRate = interestGoldRate;
        _maxInterestGold = maxInterestGold;
    }

    public override int CalculateRewradGold() => base.CalculateRewradGold() + CalculateInterest();

    int CalculateInterest()
    {
        int interestApplicableGold = Mathf.Min(_currencyManager.Gold, _maxInterestGold);
        return interestApplicableGold / GoldForInterest * _interestGoldRate;
    }
}
