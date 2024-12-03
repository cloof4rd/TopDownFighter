using NUnit.Framework;
using UnityEngine;

public class TestSkillInput
{
    [Test]
    public void Input()
    {
        bool isNeedCastSkill = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC↑", () => { isNeedCastSkill = true; Debug.Log("Cast ABC↑"); });
        c.AddSkill("BCD↓", () => { Debug.Log("Cast BCD↓"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('A');
        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.Tick(Time.deltaTime);
        c.HanleInput('↑');
        c.Tick(Time.deltaTime);


        Assert.AreEqual(true, isNeedCastSkill);
    }

    [Test]
    public void Input2()
    {
        bool isNeedCastSkill = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC↑", () => { Debug.Log("Cast ABC↑"); });
        c.AddSkill("BCD↓", () => { isNeedCastSkill = true; Debug.Log("Cast BCD↓"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.Tick(Time.deltaTime);
        c.HanleInput('D');
        c.HanleInput('↓');
        Assert.AreEqual(true, isNeedCastSkill);
    }

    [Test]
    public void Input3()
    {
        bool isNeedCastSkill1 = false, isNeedCastSkill2 = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC↑", () => { isNeedCastSkill1 = true; Debug.Log("Cast ABC↑"); });
        c.AddSkill("ABC↑↑", () => { isNeedCastSkill2 = true; Debug.Log("ABC↑↑"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('A');
        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.HanleInput('↑');
        c.HanleInput('↑');
        Assert.AreEqual(true, isNeedCastSkill1 & isNeedCastSkill2);
    }

    [Test]
    public void Input3_Timeout()
    {
        bool isNeedCastSkill1 = false, isNeedCastSkill2 = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC↑", () => { isNeedCastSkill1 = true; Debug.Log("Cast ABC↑"); });
        c.AddSkill("ABC↑↑", () => { isNeedCastSkill2 = true; Debug.Log("ABC↑↑"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('A');
        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.HanleInput('↑');
        c.Tick(10.0f);
        c.HanleInput('↑');
        Assert.AreEqual(false, isNeedCastSkill1 && isNeedCastSkill2);
    }
}
