using UnityEngine;

public class TestTask : PlayerTask
{
    public override void CompleteTask()
    {
        base.CompleteTask();
        Debug.Log("WOW");
    }
}
