using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TheoryQuizManagerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestBoolean()
    {
        TheoryQuizManager theoryQuizManager = new TheoryQuizManager();
        Assert.AreEqual(true, theoryQuizManager.GetBooleanValue("qz_1_a_1_active"), "Answer A should be active");
        Assert.AreNotEqual(false, theoryQuizManager.GetBooleanValue("qz_1_b_1_active"), "Answer B should not be inactive");
    }


    [Test]
    public void TestAnswer()
    {
        TheoryQuizManager theoryQuizManager = new TheoryQuizManager();
        Assert.AreEqual("b", theoryQuizManager.GetStringValue("qz_1_solution_1"), "The soulution is Answer B");
        Assert.AreNotEqual("c", theoryQuizManager.GetStringValue("qz_1_solution_1"), "The soulution should be Answer B and not C");

    }
}
