using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomThingsII : MonoBehaviour
{
    public GameObject Wróg;
    public GameObject Zdrowie;
    public GameObject CzerwonyWrog;
    public int iloscniebieskich;
    public int iloscapteczek;
    public int iloscczerwonych;
    private float xPos;
    private float zPos;
    private int IloscWrogow;


    void Start()
    {
        StartCoroutine(UpuszczanieWrogow());
        StartCoroutine(HealthDrop());
        StartCoroutine(UpuszczanieCzerwonychWrogow());
    }

    IEnumerator UpuszczanieWrogow()
    {

        while (IloscWrogow < iloscniebieskich)
        {
            xPos = Random.Range(-8, 8);
            zPos = Random.Range(8, -8);
            Instantiate(Wróg, new Vector3(xPos, 5, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            IloscWrogow += 1;
        }
    }

    IEnumerator HealthDrop()
    {
        while (IloscWrogow < iloscapteczek)
        {
            xPos = Random.Range(-8, 8);
            zPos = Random.Range(8, -8);
            Instantiate(Zdrowie, new Vector3(xPos, 5, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            IloscWrogow += 1;
        }
    }

    IEnumerator UpuszczanieCzerwonychWrogow()
    {
        while (IloscWrogow < iloscczerwonych)
        {
            xPos = Random.Range(-8, 8);
            zPos = Random.Range(8, -8);
            Instantiate(CzerwonyWrog, new Vector3(xPos, 5, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            IloscWrogow += 1;
        }
    }
}
