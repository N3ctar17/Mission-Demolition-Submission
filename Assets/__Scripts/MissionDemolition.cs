using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private Singleton

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; 
    // Start is called before the first frame update
    void Start()
    {
        S = this;

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        // Get rid of the old castle if one exists
        if (castle != null) {
            Destroy(castle);
        }

        // Destroy old projectiles if they exist (the method is not yet written)
        Projectile.DESTROY_PROJECTILES();

        // Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        // Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        // Zoom out to show both
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI() {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        // Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet) {
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            // Zoom out to show both
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax) {
            SceneManager.LoadSceneAsync("_GameOver");
        }
        StartLevel();
    }

    static public void SHOT_FIRED() {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE() {
        return S.castle;
    }
}
