using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class MenuButton : MonoBehaviour
{
    public static Texture2D cursorDefault;
    public static Texture2D cursorPointer;

    private void Awake()
    {
        cursorDefault = Resources.Load<Texture2D>("Cursors/default");
        cursorPointer = Resources.Load<Texture2D>("Cursors/pointer");
        Cursor.SetCursor(cursorDefault, new Vector2(0,0), CursorMode.Auto);
    }

    [SerializeField]
    public UnityEvent OnClick = new UnityEvent();

    [SerializeField] private bool blinkBefore = false;
    
    [SerializeField] 
    private TMP_Text Text;
    
    private void OnMouseUp()
    {
        if (blinkBefore)
        {
            MainLight.Instance.Blink(() =>
            {
                OnClick.Invoke();
            });
        }
        else 
            OnClick.Invoke();
    }

    public void Click()
    {
        OnMouseUp();
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(cursorDefault, new Vector2(0,0), CursorMode.Auto);
        RemoveTextStyle(FontStyles.Underline);
    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(cursorPointer, new Vector2(0,0), CursorMode.Auto);
        AddTextStyle(FontStyles.Underline);   
    }


    public void AddTextStyle(FontStyles style)
    {
        if (Text)
            Text.fontStyle |= style;
    }
    public void RemoveTextStyle(FontStyles style)
    {
        if (Text) 
            Text.fontStyle &= ~style;
    }

}
