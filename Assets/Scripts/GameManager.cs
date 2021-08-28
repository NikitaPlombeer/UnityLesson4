using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool isLose { get; private set; }

    [FormerlySerializedAs("RatsCount")] public int ratsCount = 20;
    [FormerlySerializedAs("RatPrefab")] public GameObject ratPrefab;
    [FormerlySerializedAs("Boundaries")] public Rect boundaries;
    [FormerlySerializedAs("LoseSound")] public AudioSource loseSound;
    [FormerlySerializedAs("DeathSound")] public AudioSource deathSound;
    [FormerlySerializedAs("MainLight")] public Light mainLight;
    [FormerlySerializedAs("EndText")] public Text endText;

    private readonly List<GameObject> _ratsList = new List<GameObject>();

    private int _killedCount;
    private int _savedCount;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < ratsCount; i++)
        {
            var ratPosition = getRandomPositionWithinBoundaries();
            _ratsList.Add(Instantiate(ratPrefab, ratPosition, Quaternion.identity));
        }
    }

    private Vector3 getRandomPositionWithinBoundaries()
    {
        var x = Random.Range(boundaries.xMin + 1, boundaries.xMax - 1);
        var y = Random.Range(boundaries.yMin + 1, boundaries.yMax - 1);
        return new Vector3(x, -0.5f, y);
    }

    public Vector3 FindClosestRatPositionTo(Vector3 to)
    {
        var minDistance = Mathf.Infinity;
        var closestPos = Vector3.zero;
        foreach (var rat in _ratsList)
        {
            var position = rat.transform.position;
            var distance = Vector3.Distance(position, to);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPos = position;
            }
        }

        return closestPos;
    }

    public void Lose()
    {
        if (isLose) return;

        isLose = true;
        mainLight.gameObject.SetActive(false);
        loseSound.Play();
    }

    public void RestartGame()
    {
        isLose = false;
        mainLight.gameObject.SetActive(true);
    }

    public void KillRat(GameObject rat)
    {
        _killedCount++;
        deathSound.Play();
        _ratsList.Remove(rat);
        Destroy(rat);
        CheckForGameEnd();
    }

    public void SaveRat(GameObject rat)
    {
        _savedCount++;
        _ratsList.Remove(rat);
        Destroy(rat, 7f);
        CheckForGameEnd();
    }

    private void CheckForGameEnd()
    {
        if (_ratsList.Count != 0) return;

        if (_savedCount == 0)
        {
            endText.text = "Зачем ты их всех убил?";
        }
        else if (_killedCount > _savedCount)
        {
            endText.text = "Ты любишь убивать?";
        }
        else if (_killedCount != 0)
        {
            endText.text = "Думаешь ты хорошой человек?";
        }
        else
        {
            endText.text = "Ты лучше, чем многие о тебе думают!";
        }

        Destroy(FindObjectOfType<BallController>());
        Destroy(FindObjectOfType<FearVoice>());
        endText.gameObject.SetActive(true);
    }
}