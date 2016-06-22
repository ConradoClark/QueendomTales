using UnityEngine;
using System.Collections;

public class TestRightButton : MonoBehaviour {

    public Transform deactivateThis;
    public Transform activateThis;
    public Transform activateThisLater;
    public Transform stopThingy;

    bool bla = false;
    bool bla2 = false;
    private Coroutine blaCor;
    private Coroutine blaCor2;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.RightArrow)){
            blaCor = StartCoroutine(setbla());
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            bla2 = true;
            blaCor2 = StartCoroutine(setbla2());
        }

        if (Input.GetKey(KeyCode.RightArrow))
        { 
            if (blaCor2 != null)
            {
                StopCoroutine(blaCor2);
            }
        
            bla2 = false;
            if (!bla)
            {
                deactivateThis.gameObject.SetActive(false);
                activateThis.gameObject.SetActive(false);
                activateThisLater.gameObject.SetActive(true);
            }
            else
            {
                deactivateThis.gameObject.SetActive(false);
                activateThisLater.gameObject.SetActive(false);
                activateThis.gameObject.SetActive(true);
            }
        }
        else
        {
            if (blaCor != null)
            {
                StopCoroutine(blaCor);
            }
            bla = false;

            if (bla2)
            {
                deactivateThis.gameObject.SetActive(false);
                activateThis.gameObject.SetActive(false);
                stopThingy.gameObject.SetActive(true);
                activateThisLater.gameObject.SetActive(false);                
            }
            else
            {
                deactivateThis.gameObject.SetActive(true);
                activateThis.gameObject.SetActive(false);
                activateThisLater.gameObject.SetActive(false);
                stopThingy.gameObject.SetActive(false);
            }
        }
	}

    IEnumerator setbla()
    {
        yield return new WaitForSeconds(0.2f);
        bla = true;
    }

    IEnumerator setbla2()
    {
        yield return new WaitForSeconds(0.2f);
        bla2 = false;
    }
}
