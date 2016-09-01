using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour
{
    public GameObject characterTemplate;
    public GameObject uiTemplate;
    public Character character { get; private set; }
    public CharacterUI characterUI { get; private set; }

    private bool init = false, ready = false;

    void Start()
    {
        uiTemplate = Resources.Load<GameObject>("Prefabs/Character/01C - MainCharacter UI");
        characterTemplate = Resources.Load<GameObject>("Prefabs/Character/01C - MainCharacter");

        GameObject ui = Instantiate(uiTemplate);
        GameObject player = Instantiate(characterTemplate);

        this.characterUI = ui.GetComponent<CharacterUI>();
        this.character = player.GetComponent<Character>();

        this.character.ConnectUI(characterUI);

        StartCoroutine(Initialize());
    }

    public bool IsInitialized()
    {
        return ready;
    }

    private IEnumerator Initialize()
    {
        yield return 1;
        init = true;
        yield return 1;
        ready = true;
    }

    public IEnumerator WaitForInitialization()
    {
        while (!init)
        {
            yield return 1;
        }
    }
}
