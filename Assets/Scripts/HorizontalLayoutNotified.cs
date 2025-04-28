using UnityEngine.Events;
using UnityEngine.UI;

public class HorizontalLayoutNotified : HorizontalLayoutGroup
{
    public UnityEvent onLayoutChanged;

    public override void SetLayoutHorizontal()
    {
        base.SetLayoutHorizontal();
        onLayoutChanged?.Invoke();
    }

    public override void SetLayoutVertical()
    {
        base.SetLayoutVertical();
        onLayoutChanged?.Invoke();
    }
}
