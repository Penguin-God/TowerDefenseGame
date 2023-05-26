using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MultiCurrencyTests
{
    const byte MasterID = 0;
    const byte ClientID = 1;

    MasterCurrencyManager SetupController(out ServerManager container)
    {
        var sut = new GameObject("").AddComponent<MasterCurrencyManager>();
        container = new ServerManager();
        sut.Init(container);
        return sut;
    }

    [Test]
    [TestCase(MasterID, ClientID)]
    [TestCase(ClientID, MasterID)]
    public void TestAddFiveGold(byte sutID, byte otherID)
    {
        var sut = SetupController(out var dataContainer);

        const byte Five = 5;
        sut.AddGold(Five, sutID);
        Assert.AreEqual(Five, dataContainer.GetBattleData(sutID).Gold);
        Assert.AreEqual(0, dataContainer.GetBattleData(otherID).Gold);
    }

    [Test]
    [TestCase(MasterID, ClientID)]
    [TestCase(ClientID, MasterID)]
    public void TestAddFiveFood(byte sutID, byte otherID)
    {
        var sut = SetupController(out var dataContainer);

        const byte Five = 5;
        sut.AddFood(Five, sutID);
        Assert.AreEqual(Five, dataContainer.GetBattleData(sutID).Food);
        Assert.AreEqual(0, dataContainer.GetBattleData(otherID).Food);
    }

    [Test]
    [TestCase(MasterID, ClientID)]
    [TestCase(ClientID, MasterID)]
    public void TestUseFiveGold(byte sutID, byte otherID)
    {
        var sut = SetupController(out var dataContainer);

        const byte Five = 5;
        sut.AddGold(3, sutID);
        sut.UseGold(Five, sutID);
        Assert.AreEqual(3, dataContainer.GetBattleData(sutID).Gold);

        sut.AddGold(3, sutID);
        sut.UseGold(Five, sutID);
        Assert.AreEqual(1, dataContainer.GetBattleData(sutID).Gold);
        Assert.AreEqual(0, dataContainer.GetBattleData(otherID).Gold);
    }

    [Test]
    [TestCase(MasterID, ClientID)]
    [TestCase(ClientID, MasterID)]
    public void TestUseFiveFood(byte sutID, byte otherID)
    {
        var sut = SetupController(out var dataContainer);

        const byte Five = 5;
        sut.AddFood(3, sutID);
        sut.UseFood(Five, sutID);
        Assert.AreEqual(3, dataContainer.GetBattleData(sutID).Food);

        sut.AddFood(3, sutID);
        sut.UseFood(Five, sutID);
        Assert.AreEqual(1, dataContainer.GetBattleData(sutID).Food);
        Assert.AreEqual(0, dataContainer.GetBattleData(otherID).Food);
    }
}
