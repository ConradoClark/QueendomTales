using UnityEngine;
using System.Collections;

public class BattleTest : MonoBehaviour {

    public AudioSource levelSong;
    public AudioSource battleSong;
    private bool battle;
    public MeshRenderer[] borders;
    private float lerp;
    private bool changing;
    public Material levelBorder;
    public Material battleBorder;
    private Color originalAmbient;
    public Color battleLight;

    void Start () {
        levelSong.Play();
        battleSong.Play();
        levelSong.volume = 1;
        battleSong.volume = 0;
        originalAmbient = RenderSettings.ambientLight;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            battleSong.time = levelSong.time;
            if (battle)
            {
                battle = false;
                changing = true;
            }
            else
            {
                battle = true;
                changing = true;
            }
        }

        if (changing)
        {
            AudioSource source = levelSong;
            AudioSource target = battleSong;

            source.volume = 1-lerp;
            target.volume = lerp;

            lerp += Time.deltaTime * (battle ? 1 : -1);
            lerp = Mathf.Clamp(lerp, 0, 1);

            for (int i = 0; i < borders.Length; i++)
            {                
                borders[i].material.Lerp(levelBorder, battleBorder, lerp*lerp);
                float origOp = borders[i].material.GetFloat("_Opacity");
                float opVal = Mathf.Abs(lerp - origOp);
                borders[i].material.SetFloat("_Opacity", Mathf.Clamp(opVal,0,origOp));
            }

            RenderSettings.ambientLight = Color.Lerp(originalAmbient, battleLight, lerp);
        }

        if (lerp==0 || lerp == 1)
        {
            changing = false;
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].material.Lerp(levelBorder, battleBorder, lerp * lerp);
            }
        }
	}
}
