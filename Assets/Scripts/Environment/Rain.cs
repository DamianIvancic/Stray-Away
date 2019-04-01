using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

    public Raindrop RaindropPrefab;
    public GameObject LightningTilemap;
    public AudioSource ThunderSound;

    private Transform _transform;
    private Camera _cam;

    private Raindrop[] Raindrops = new Raindrop[1000];

    private float _timer = 0f;
    private float _lightningPeriod = 10f;

    void Start()
    {
        _transform = transform;
        _cam = Camera.main;

        for(int i=0; i<1000; i++)
        {
            Vector3 worldPos = _cam.ScreenToWorldPoint(new Vector3(Random.Range(Screen.width*-1, Screen.width*2), Random.Range(0, Screen.height*10), 0));
            Vector3 pos = new Vector3(worldPos.x, worldPos.y, 0);
               
            Raindrops[i] = Instantiate(RaindropPrefab.gameObject, pos, Quaternion.Euler(0,0,150), gameObject.transform).GetComponent<Raindrop>();
            if (pos.y < Screen.height)
                Raindrops[i].RB.velocity = new Vector2(0, Random.Range(-5, -1));
        }
    }

    void Update()
    {
        for (int i = 0; i < 1000; i++)
        {         
            Vector3 pos = _cam.WorldToScreenPoint(Raindrops[i].transform.position);

            if (pos.y < 0)
            {
                Vector3 worldPos = _cam.ScreenToWorldPoint(new Vector3(Random.Range(Screen.width*-1, Screen.width*2), Random.Range(Screen.height, Screen.height*2), 0));
                pos.x = worldPos.x;
                pos.y = worldPos.y;
                pos.z = 0;
                Raindrops[i].transform.position = pos;
                Raindrops[i].RB.velocity = Vector2.zero;
            }
        }

        _timer += Time.deltaTime;
        if(_timer >= _lightningPeriod)
        {
            _timer = 0f;
            StartCoroutine(Thunder());
        }
    }

    IEnumerator Thunder()
    {
        LightningTilemap.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        LightningTilemap.SetActive(false);
        ThunderSound.Play();
        yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
        LightningTilemap.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        LightningTilemap.SetActive(false);

        float chance = Random.Range(0, 100);
        if (chance >= 85)
        {
            _lightningPeriod = Random.Range(25, 45);

            while (ThunderSound.isPlaying)
                yield return null;

            yield return new WaitForSeconds(3f);
            LightningTilemap.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            LightningTilemap.SetActive(false);
            ThunderSound.Play();
            yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
            LightningTilemap.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            LightningTilemap.SetActive(false);        
        }
        else
        {
            _lightningPeriod = Random.Range(10, 45);
            yield return null;
        }
    }
}
