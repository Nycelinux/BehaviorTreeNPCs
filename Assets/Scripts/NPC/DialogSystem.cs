using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChooseOption(int option)
    {
        switch (option)
        {
            case 1:
                GameManager.instance.reputation += 10;
                Debug.Log("Du bittest um Segen");
                break;
            case 2:
                GameManager.instance.reputation += 5;
                UIManager.instance.resources -= 5;
                Debug.Log("Du bringst ein Opfer");
                break;
            case 3:
                GameManager.instance.reputation -= 5;
                Debug.Log("Du ignoriert die Priesterin");
                break;
        }
    }

  
}
