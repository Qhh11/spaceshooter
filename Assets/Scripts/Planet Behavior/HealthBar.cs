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
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar = this.GetComponent<Slider>();
        healthBar.maxValue = maxHP;
        healthBar.value = maxHP;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
	    //Game Over: Planet zerstört
	    if (currentHP <= 0)
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
        healthBar.value = hp;

		if(currentHP > maxHP*0.4f){
			inner.color = new Color(0f, 1f, 0f, 1f);
		}else if(currentHP <= maxHP*0.4f && currentHP > maxHP*0.2f){
			inner.color = new Color(1f, 0.92f, 0.016f, 1f);
		}else{
			inner.color = new Color(1f, 0f, 0f, 1f);
		}
    }

    //falls Planet-HP 0: End-Sequenz !!!NOT WORKING 
    public void DestroyPlanet()
    {
	    earth.DestroyItself();
    }

	//Getter
	public float getCurrentHP(){
		return currentHP;
	}

}