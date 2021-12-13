using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisksManager : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabDisk = null;
    [SerializeField]
    private Transform minSpawnPos = null;
    [SerializeField]
    private Transform maxSpawnPos = null;
    [SerializeField]
    private TextMeshPro textScore = null;
    [SerializeField]
    private TextMeshPro textHighScore = null;
    [SerializeField]
    private TextMeshPro textTimeLeft = null;
    [SerializeField]
    private Collider[] forbiddenColliders = null;

    private GameObject instDisk = null;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;

        spawDisk();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = -10f;
                Score = 0;

                if (instDisk != null)
                {
                    Destroy(instDisk);
                }
            }

            textTimeLeft.text = "Time left: " + timer.ToString("n2");
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= 0f)
            {
                timer = 90f;
                spawDisk();
            }
            else
            {
                textTimeLeft.text = "New game in: " + (-timer).ToString("n2");
            }
        }
    }

    private void spawDisk()
    {
        if (instDisk != null)
        {
            instDisk.GetComponent<HitableDisk>().DiskHit -= DisksManager_DiskHit;
            Destroy(instDisk);
        }

        instDisk = Instantiate(prefabDisk);

        Vector3 randPos = Vector3.zero;

        do
        {
            randPos = new Vector3(Mathf.Lerp(minSpawnPos.position.x, maxSpawnPos.position.x, Random.Range(0f, 1f)),
                                                    Mathf.Lerp(minSpawnPos.position.y, maxSpawnPos.position.y, Random.Range(0f, 1f)),
                                                    Mathf.Lerp(minSpawnPos.position.z, maxSpawnPos.position.z, Random.Range(0f, 1f)));

        }
        while (distanceToForbiddenColliders(randPos) <= 0.3f);

        instDisk.transform.position = randPos;

        instDisk.transform.rotation = Quaternion.identity;
        instDisk.GetComponent<HitableDisk>().DiskHit += DisksManager_DiskHit;
    }

    private float distanceToForbiddenColliders(Vector3 pos)
    {
        float closestDistance = float.MaxValue;
        for (int i = 0; i < forbiddenColliders.Length; i++)
        {
            Vector3 closestPoint = forbiddenColliders[i].ClosestPoint(pos);

            if (Vector3.Distance(closestPoint, pos) < closestDistance)
            {
                closestDistance = Vector3.Distance(closestPoint, pos);
            }
        }

        return closestDistance;
    }

    private void DisksManager_DiskHit(HitableDisk disk)
    {
        Score++;
        Invoke("spawDisk", 1f);
    }

    private int highscore = 0;
    private int ownScore = 0;
    public int Score
    {
        get
        {
            return ownScore;
        }
        set
        {
            int oldScore = ownScore;
            ownScore = value;

            if (oldScore != ownScore)
            {
                textScore.text = "Hits: " + ownScore.ToString();
            }

            if (ownScore > highscore)
            {
                highscore = ownScore;
                textHighScore.text = "Highscore: " + highscore.ToString();
            }
        }
    }
}
