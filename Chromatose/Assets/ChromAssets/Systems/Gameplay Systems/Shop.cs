using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	
	public enum requiredCollTypeEnum{
		White, Red, Blue
	}
	
	public requiredCollTypeEnum requiredCollType;
	
	public int requiredPayment;
	public int minCollRequBefOpenShop;
	
	private bool inZone;
	
	private BoxCollider blocker;
	
	
	void Start () {
		blocker = GetComponentInChildren<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		inZone = GetComponentInChildren<ShopDetection>().onDetectZone;	
	
		switch(requiredCollType){
		case requiredCollTypeEnum.White:
			
			if(inZone && StatsManager.whiteCollDisplayed < minCollRequBefOpenShop){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles blancs avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.whiteCollDisplayed >= minCollRequBefOpenShop && StatsManager.whiteCollDisplayed < (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				Debug.Log(requiredPayment + " Collectibles blancs sont requis pour ouvrir");
			}
			else if (inZone && StatsManager.whiteCollDisplayed >= (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			
			break;
		case requiredCollTypeEnum.Red:
			
			if(inZone && StatsManager.redCollDisplayed < minCollRequBefOpenShop){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles rouges avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.redCollDisplayed >= minCollRequBefOpenShop && StatsManager.redCollDisplayed < (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				Debug.Log(requiredPayment + " Collectibles rouges sont requis pour ouvrir");
			}
			else if (inZone && StatsManager.redCollDisplayed >= (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			break;
		case requiredCollTypeEnum.Blue:
			
			if(inZone && StatsManager.blueCollDisplayed < minCollRequBefOpenShop){
					//Affiche le bubble qui indique le nb de coll necesaire avant que le shop soit disponible
				Debug.Log("Il vous faut minimum " + minCollRequBefOpenShop + " Collectibles bleus avant de pouvoir activer le shop");
			}
			else if(inZone && StatsManager.blueCollDisplayed >= minCollRequBefOpenShop && StatsManager.blueCollDisplayed < (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique cmb de coll pour acheter l'ouverture
				Debug.Log(requiredPayment + " Collectibles bleus sont requis pour ouvrir");
			}
			else if (inZone && StatsManager.blueCollDisplayed >= (minCollRequBefOpenShop + requiredPayment)){
					//Affiche le bubble qui indique qu'on peut effectuer l'ouverture
				Debug.Log("Ouvrez la barriere avec P");
			}
			
			break;
		}
		
		
	}
}
