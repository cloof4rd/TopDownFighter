using NUnit.Framework;
using UnityEngine;

public class TestSkillInput
{
    [Test]
    public void Input()
    {
        bool isNeedCastSkill = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC¡ü", () => { isNeedCastSkill = true; Debug.Log("Cast ABC¡ü"); });
        c.AddSkill("BCD¡ý", () => { Debug.Log("Cast BCD¡ý"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('A');
        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.Tick(Time.deltaTime);
        c.HanleInput('¡ü');
        c.Tick(Time.deltaTime);


        Assert.AreEqual(true, isNeedCastSkill);
    }

    [Test]
    public void Input2()
    {
        bool isNeedCastSkill = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC¡ü", () => { Debug.Log("Cast ABC¡ü"); });
        c.AddSkill("BCD¡ý", () => { isNeedCastSkill = true; Debug.Log("Cast BCD¡ý"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.Tick(Time.deltaTime);
        c.HanleInput('D');
        c.HanleInput('¡ý');
        Assert.AreEqual(true, isNeedCastSkill);
    }

    [Test]
    public void Input3()
    {
        bool isNeedCastSkill1 = false, isNeedCastSkill2 = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC¡ü", () => { isNeedCastSkill1 = true; Debug.Log("Cast ABC¡ü"); });
        c.AddSkill("ABC¡ü¡ü", () => { isNeedCastSkill2 = true; Debug.Log("ABC¡ü¡ü"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('A');
        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.HanleInput('¡ü');
        c.HanleInput('¡ü');
        Assert.AreEqual(true, isNeedCastSkill1 & isNeedCastSkill2);
    }

    [Test]
    public void Input3_Timeout()
    {
        bool isNeedCastSkill1 = false, isNeedCastSkill2 = false;
        SkillInputController c = new SkillInputController();
        c.AddSkill("ABC¡ü", () => { isNeedCastSkill1 = true; Debug.Log("Cast ABC¡ü"); });
        c.AddSkill("ABC¡ü¡ü", () => { isNeedCastSkill2 = true; Debug.Log("ABC¡ü¡ü"); });

        c.Tick(Time.deltaTime);
        c.HanleInput('A');
        c.Tick(Time.deltaTime);
        c.HanleInput('B');
        c.Tick(Time.deltaTime);
        c.HanleInput('C');
        c.HanleInput('¡ü');
        c.Tick(10.0f);
        c.HanleInput('¡ü');
        Assert.AreEqual(false, isNeedCastSkill1 && isNeedCastSkill2);
    }
}
