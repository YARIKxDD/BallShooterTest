using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : BehaviourSingleton<GameManager>
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballStartPoint;
    [SerializeField] private Transform finishTransform;
    [SerializeField] private GameObject expolsionPrefab;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform explosionsPool;
    [SerializeField] private LayerMask checkLayerMask;

    [SerializeField] private GameObject tutorialObj;
    [SerializeField] private GameObject waitTapObj;
    [SerializeField] private GameObject waitResultObj;
    [SerializeField] private GameObject winObj;
    [SerializeField] private GameObject loseObj;

    [SerializeField] private ChangeVolumeType changeVolumeType;

    [SerializeField] private float explosionCooldown;
    [SerializeField] private float volumeChangeK;
    [SerializeField] private float minimalSize;
    [SerializeField] private float debugSize;
    [SerializeField] private float ballSpeed;
    [SerializeField] private float playerSpeed;

    private List<GameObject> EnemiesToDestroy = new List<GameObject>();

    private GameState _currentGameState;
    public GameState CurrentGameState
    {
        get
        {
            return _currentGameState;
        }
        set
        {
            _currentGameState = value;
            tutorialObj.SetActive(value == GameState.Tutorial);
            winObj.SetActive(value == GameState.Win);
            loseObj.SetActive(value == GameState.Lose);
            waitTapObj.SetActive(value == GameState.WaitTap);
            waitResultObj.SetActive(value == GameState.WaitResult);
        }
    }

    private Ball currentBall;
    private float currentScale = 0;

    private void Start()
    {
        Application.targetFrameRate = 60;
        CurrentGameState = GameState.Tutorial;
        tutorialObj.SetActive(true);

        StartCoroutine(BoomRoutine());
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Tutorial || CurrentGameState == GameState.WaitTap)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.mousePosition.y / Screen.height < 0.9f)
                {
                    if (CurrentGameState == GameState.Tutorial)
                    {
                        tutorialObj.SetActive(false);
                    }
                    StartCharge();
                }
            }
        }
        else if (CurrentGameState == GameState.Tap)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Shot();
            }
            else
            {
                Charge();
            }
        }
    }

    public void StopWaitResult()
    {
        RaycastHit raycastHit;
        if (Physics.SphereCast(playerTransform.position, playerTransform.localScale.x / 2, new Vector3(0, 0, 1), out raycastHit, 100, checkLayerMask))
        {
            if (raycastHit.collider.gameObject.layer == Layers.Enemy)
            {
                CurrentGameState = GameState.WaitTap;
            }
            if (raycastHit.collider.gameObject.layer == Layers.Finish)
            {
                CurrentGameState = GameState.ToWin;
                StartCoroutine(MoveToFinishRoutine());
            }
        }
        else
        {
            CurrentGameState = GameState.Lose;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheckToDestroy(GameObject enemy)
    {
        if (!EnemiesToDestroy.Contains(enemy))
        {
            EnemiesToDestroy.Add(enemy);
            enemy.GetComponent<Enemy>().SetReadyToDestroy();
        }
    }

    private void StartCharge()
    {
        CurrentGameState = GameState.Tap;
        GameObject newBall = Instantiate(ballPrefab, ballStartPoint);
        currentBall = newBall.GetComponent<Ball>();
        currentBall.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Charge()
    {
        float dv = Time.deltaTime * volumeChangeK;
        ChangeVolume(currentBall.transform, dv);
        ChangeVolume(playerTransform, -dv);

        currentScale = currentBall.transform.localScale.x;

        if (playerTransform.localScale.x < minimalSize)
        {
            GameOverBySize();
        }
    }

    private void Shot()
    {
        CurrentGameState = GameState.WaitResult;

        Vector3 offset = finishTransform.position - currentBall.transform.position;

        currentBall.OnShot(offset.normalized * ballSpeed);
    }

    private void GameOverBySize()
    {
        CurrentGameState = GameState.Lose;
        if (currentBall != null)
        {
            Destroy(currentBall.gameObject);
        }
    }

    private IEnumerator MoveToFinishRoutine()
    {
        Vector3 startPos = playerTransform.position;
        Vector3 endPos = finishTransform.position;
        float time = (endPos - startPos).magnitude / playerSpeed;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            playerTransform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return null;
        }
        CurrentGameState = GameState.Win;
    }

    private IEnumerator BoomRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(explosionCooldown);
            if (currentBall != null)
            {
                continue;
            }
            if (EnemiesToDestroy.Count > 0)
            {
                GameObject enemy = EnemiesToDestroy[0];

                Vector3 pos = enemy.transform.position;

                EnemiesToDestroy.RemoveAt(0);
                Destroy(enemy);

                GameObject newExplosion = Instantiate(expolsionPrefab, pos, Quaternion.identity, explosionsPool);
                newExplosion.transform.localScale *= currentScale;
            }
            else
            {
                if (CurrentGameState == GameState.WaitResult)
                {
                    if (explosionsPool.childCount == 0)
                        StopWaitResult();
                }
            }
        }
    }

    private void ChangeVolume(Transform transform, float dv)
    {

        float oldR = transform.localScale.x;

        float newR = oldR;

        if (changeVolumeType == ChangeVolumeType.Volume)
        {
            newR = Mathf.Pow(Mathf.Pow(oldR, 3) + 3 * dv / (4 * Mathf.PI), 1 / 3f);
        }
        else
        {
            newR = oldR + dv;
        }

        if (newR < minimalSize || float.IsNaN(newR)) newR = minimalSize / 2f;
        transform.localScale = new Vector3(1, 1, 1) * newR;

        if (transform == playerTransform)
        {
            lineRenderer.startWidth = newR;
        }
    }
}

public enum GameState
{
    Tutorial = 0,
    WaitTap,
    Tap,
    WaitResult,
    ToWin,
    Win,
    ToLose,
    Lose
}

public enum ChangeVolumeType
{
    Linear,
    Volume
}