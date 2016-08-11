using UnityEngine;
using System.Collections;

public class TestCombo : MonoBehaviour
{
    public FrostyInputActionFragment combo;
    public SpriteRenderer[] sprites;
    public GameObject alecfu;
    float step = 0f;

    void Start()
    {

    }

    void Update()
    {
        var input = combo.EvaluateInput();
        if (input.MoveNext() && input.Current)
        {
            step = 0.75f;
            var a = GameObject.Instantiate(alecfu);
            a.transform.position = this.transform.position - new Vector3(100,20,0);

        }

        if (step >= 0)
        {
            float lerp = Mathf.Lerp(1, 0, Mathf.Cos(step * 2.5f));
            foreach (var spr in sprites)
            {
                spr.material.SetFloat("_Luminance", lerp);
            }
            step -= Time.deltaTime;
        }
        else
        {
            foreach (var spr in sprites)
            {
                spr.material.SetFloat("_Luminance", 0);
            }
        }    
    }
}
