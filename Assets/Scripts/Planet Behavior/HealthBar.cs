using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
	public Image inner;
	public Planet earth;

	public float maxHP;
    private float currentHP;

    [SerializeField] private GameObject planet;

    private bool explosionPlayed;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar = this.GetComponent<Slider>();
        healthBar.maxValue = maxHP;
        healthBar.value = maxHP;
        currentHP = maxHP;
        explosionPlayed = false;
        inner.color = new Color(0f, 1f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
	    //Game Over: Planet zerstört
	    if (currentHP <= 0 && explosionPlayed==false)
	    {
		    DestroyPlanet();
	    }
	}

	//Damage für Planet
    public void DamageTaken(float dmg)
    {
        currentHP = currentHP-dmg;
        SetHealth(currentHP);
    }

	//Heal für Planet
    public void HpIncrease(float heal)
    {
        currentHP = currentHP + heal;
        SetHealth(currentHP);
    }

	//Modifizieren der Planet-HP Anzeige
    public void SetHealth(float hp)
    {
        // healthBar.value = hp;
        LeanTween.value(gameObject, healthBar.value, hp, .3f).setEase(LeanTweenType.easeSpring)
	        .setOnUpdate((float val) => healthBar.value = val);


        if(currentHP > maxHP*0.4f)
        {
	        LeanTween.color(inner.rectTransform, new Color(0, 1f, 0, 1f), .6f).setEase(LeanTweenType.linear);
        }else if(currentHP <= maxHP*0.6f && currentHP > maxHP*0.2f){
			// inner.color = new Color(1f, 0.92f, 0.016f, 1f);
			LeanTween.color(inner.rectTransform, new Color(1f, 0.92f, 0.016f, 1f), .6f).setEase(LeanTweenType.linear);
        }else{
			// inner.color = new Color(1f, 0f, 0f, 1f);
			LeanTween.color(inner.rectTransform, new Color(1f, 0f, 0f, 1f), .6f).setEase(LeanTweenType.linear);
		}
    }

    //falls Planet-HP 0: End-Sequenz
    public void DestroyPlanet()
    {
	    StartCoroutine(earth.DestroyItself());
	    explosionPlayed = true;
    }

	//Getter
	public float getCurrentHP(){
		return currentHP;
	}

}