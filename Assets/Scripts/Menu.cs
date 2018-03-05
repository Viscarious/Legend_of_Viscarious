﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    [SerializeField] private GameObject hero;
    [SerializeField] private GameObject tanker;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject ranger;

    private Animator heroAnim;
    private Animator tankerAnim;
    private Animator soldierAnim;
    private Animator rangerAnim;

    private void Awake()
    {
        Assert.IsNotNull(hero);
        Assert.IsNotNull(tanker);
        Assert.IsNotNull(soldier);
        Assert.IsNotNull(ranger);
    }

    // Use this for initialization
    void Start ()
    {
        heroAnim = hero.GetComponent<Animator>();
        tankerAnim = tanker.GetComponent<Animator>();
        soldierAnim = soldier.GetComponent<Animator>();
        rangerAnim = ranger.GetComponent<Animator>();

        StartCoroutine(Showcase());
    }
	
	
    IEnumerator Showcase()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        heroAnim.Play("DoubleChop");
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        tankerAnim.Play("Primary Attack");
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        soldierAnim.Play("Primary Attack");
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        heroAnim.Play("SpinAttack");
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        rangerAnim.Play("Primary Attack");

        StartCoroutine(Showcase());
    }

    public void Battle()
    {
        SceneManager.LoadScene("LegendOfViscarious");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
