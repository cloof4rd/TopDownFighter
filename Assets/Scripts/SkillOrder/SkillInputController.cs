using System;
using System.Collections.Generic;
public class SkillInputController
{
    private List<SkillState> skills = new List<SkillState>();
    public SkillState AddSkill(string skillStr, Action onCastSkill)
    {
        var skill = new SkillState(skillStr);
        skills.Add(skill);
        skill.OnCastSkillSuccess += onCastSkill;
        return skill;
    }
    public bool RemoveSkill(SkillState skill)
    {
        return skills.Remove(skill);
    }
    public void Tick(float deltaTime)
    {
        foreach (var skill in skills)
        {
            skill.Update(deltaTime);
        }
    }
    public void HanleInput(char inputChar)
    {
        foreach (var skill in skills)
        {
            if (skill.HandleInput(inputChar))
                break;
        }
    }
}

public class SkillState
{
    private string skillStr;
    private float timer;
    private float ClearInputTime;
    private int currentMatchIndex = 0;
    private int skillID;
    public event Action OnCastSkillSuccess;

    public SkillState(string skillStr)
    {
        this.skillStr = skillStr;
        ClearInputTime = ConstConfig.KEY_INPUT_TIME;
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
        if (timer > ClearInputTime)
            Clear();

    }
    public bool HandleInput(char inputChar)
    {
        if (!TryGetMatchChar(out var matchChar))
            return false;
        // 施法匹配成功技能表
        if (inputChar == matchChar)
        {
            currentMatchIndex++;
            timer = 0;
            if (currentMatchIndex >= skillStr.Length)
            {
                OnCastSkillSuccess?.Invoke();
                currentMatchIndex = 0;
                return true;
            }
            return false;
        }
        else
        {
            Clear();
            return false;
        }
    }
    private bool TryGetMatchChar(out char result)
    {
        result = default;
        if (skillStr.Length > 0)
        {
            result = skillStr[currentMatchIndex];
            return true;
        }
        return false;
    }
    private void Clear()
    {
        timer = 0;
        currentMatchIndex = 0;
    }


}