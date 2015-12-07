using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Item : MonoBehaviour {

	public Sprite	image;
	public string	description;
	public int		price;

	public float	healthIncrease,
					rangeIncrease,
					speedIncrease,
					fireRateIncrease,
					moneyMultiplierIncrease;

	public bool		setRatKing;
}
