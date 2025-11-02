using UnityEngine;
using UnityEngine.UI;

public class heartDisplay : MonoBehaviour
{
    public int heart;
    public int maxHearts;
    public Image[] hearts_display;

    void Start()
    {
        
    }

    void Update()
    {
        for(int i = 0;  i < hearts_display.Length; i++){
            if(i < maxHearts)
            {
                hearts_display[i].enabled = true;
            } 
            else
            {
                hearts_display[i].enabled = false;
            }
        }
    }
}
