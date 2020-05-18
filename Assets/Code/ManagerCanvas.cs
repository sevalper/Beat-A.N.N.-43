using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player
{
    public int id;
    public int score;
    public string name;

    public Player(int id, int s, string n) { this.id = id; score = s; name = n; }
}

public class ManagerCanvas : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    public Text text;
    public string[] names;
    public GameObject pauseMenu;

    //finish
    public Text textoFin;
    public Image finishCanvas;


    private void Awake()
    {
        finishCanvas.canvasRenderer.SetAlpha(1.0f);
        finishCanvas.CrossFadeAlpha(0.0f, 3, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 10f;
    }

    /*
    private void Start()
    {
        for (int i = 1; i < 5; i++)
        {
            players.Add(new Player(i,0, names[i - 1]));
        }
    }
    */
    public void ActualizeCanvasPositions(int id, int score)
    {
        foreach (var player in players)
            if (player.id == id)
                player.score += score;

        text.text = GetInOrder();
    }

    private string GetInOrder()
    {
        var aux = players.OrderByDescending(x => x.score);

        string txt = "";
        var count = 1;
        foreach (var play in aux)
        {
            txt += $"-{count}- {play.name} \n";
            count++;
        }
        return txt;
    }

    public void Finish(List<FinishPos> positions)
    {
        var txt = "Resultado: \n";
        foreach (var pos in positions)
        {
            txt += $"{pos.name}: {pos.pos} \n";
        }

        textoFin.text = txt;
        finishCanvas.CrossFadeAlpha(1.0f, 3,false);
    }

}
