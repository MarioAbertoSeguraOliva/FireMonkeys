using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIClimbTest : MonoBehaviour {

    public GameObject character;
    private Text text;

    void Start()
    {

        if (character == null)
            character = GameObject.Find("ClimbCharacter");

        character.GetComponentInChildren<ClimbController>().climbEvent += onClimb;
        text = GetComponent<Text>();
    }

    void onClimb(bool isClimbing)
    {
        text.text = (isClimbing) ? "Escalar -> E  Soltarse -> Espacio" : "";
    }

}
