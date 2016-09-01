using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleManager : MonoBehaviour {

    public AudioSource levelSong;
    public AudioSource battleSong;
    private bool battle;
    
    private float lerp;
    private bool changing;
    public Material levelBorder;
    public Material battleBorder;
    private Color originalAmbient;
    public Color battleLight;
    public Color hpCounterColor;
    public TimeLayers timeLayer;

    private MeshRenderer[] borders;
    private SpriteRenderer hpCounterBorder;
    private Color originalHpCounterColor;
    private CharacterManager characterManager;
    private BasicEnemy[] currentEnemies;

    IEnumerator Start () {
        characterManager = QTToolbox.Instance.characterManager;
        yield return characterManager.WaitForInitialization();

        CharacterUI ui = characterManager.characterUI;
        hpCounterBorder = ui.HPCounter.border;
        borders = new[] { ui.TopBorder, ui.BottomBorder, ui.LeftBorder, ui.RightBorder };

        if (!levelSong.isPlaying)
        {
            levelSong.Play();
        }
        battleSong.Play();
        levelSong.volume = 1;
        battleSong.volume = 0;
        originalAmbient = RenderSettings.ambientLight;
        originalHpCounterColor = hpCounterBorder.material.GetColor("_Colorize");
    }
	
    public void StartBattle(BasicEnemy[] enemies)
    {
        battleSong.time = levelSong.time;
        battle = true;
        changing = true;
        currentEnemies = enemies;
    }

    protected void EndBattle()
    {
        battleSong.time = levelSong.time;
        battle = false;
        changing = true;
    }

    public bool IsInBattle()
    {
        return battle;
    }

	void Update () {
        if (!characterManager.IsInitialized()) return;

        if (currentEnemies!= null && currentEnemies.All(e => e.IsDead()) && battle)
        {
            EndBattle();
        }

        if (changing)
        {
            AudioSource source = levelSong;
            AudioSource target = battleSong;

            source.volume = 1-lerp;
            target.volume = lerp;

            lerp += Toolbox.Instance.time.GetDeltaTime(timeLayer) * (battle ? 1 : -1);
            lerp = Mathf.Clamp(lerp, 0, 1);

            for (int i = 0; i < borders.Length; i++)
            {                
                borders[i].material.Lerp(levelBorder, battleBorder, lerp*lerp);
                float origOp = borders[i].material.GetFloat("_Opacity");
                float opVal = Mathf.Abs(lerp - origOp);
                borders[i].material.SetFloat("_Opacity", Mathf.Clamp(opVal,0,origOp));
            }

            if (hpCounterBorder != null)
            {
                hpCounterBorder.material.SetColor("_Colorize", Color.Lerp(originalHpCounterColor, hpCounterColor, lerp));
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
