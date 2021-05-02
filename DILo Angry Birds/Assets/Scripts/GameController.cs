using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TrailController TrailController;
    public SlingShooter SlingShooter;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    public BoxCollider2D TapCollider;

    public GameObject GameOver;
    public GameObject GameWin;

    private Bird _shotBird;
    private bool _isGameEnded = false;

   

    private void Start()
    {
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for (int i=0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        SlingShooter.InstantiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Enemies.Count == 0)
        {
            //masukin buat input kode disini
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Level 2");
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Level 1");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if (Birds.Count > 0)
            SlingShooter.InstantiateBird(Birds[0]);

        if (Birds.Count == 0)
        {
            if (Birds.Count == 0 && Enemies.Count == 0)
            {
                _isGameEnded = true;
                GameWin.SetActive(true);
            } 
            else if (Birds.Count > Enemies.Count && Enemies.Count == 0)
            {
                _isGameEnded = true;
                GameWin.SetActive(true);
            } 
            else if (Birds.Count == 0 && Birds.Count < Enemies.Count)
            {
                _isGameEnded = true;
                GameOver.SetActive(true);
            }
            
            
        }

    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0)
        {
            _isGameEnded = true;
            //masukin kode buat UI disini
            GameWin.SetActive(true);

        }
        
    }

    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    private void OnMouseUp()
    {
        if (_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}
