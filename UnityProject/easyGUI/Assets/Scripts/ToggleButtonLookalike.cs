using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ToggleButtonLookalike : MonoBehaviour {

	public Image buttonBackground;
	Color untoggledColor;
	Color toggledColor;
	bool isChecked;

	// Use this for initialization
	void Start () 
	{
		isChecked = true;
		ColorUtility.TryParseHtmlString ("#2F2F2F00", out untoggledColor);
		ColorUtility.TryParseHtmlString ("#2F2F2F52", out toggledColor);
	}
	
	// Update is called once per frame
	// Change the Color of the Background based on the boolean isChecked
	void Update () 
	{
		if (isChecked)
			buttonBackground.color = toggledColor;
		else
			buttonBackground.color = untoggledColor;
	}

	// Setter for is Checked
	public void setChecked(bool stateIn)
	{
		isChecked = stateIn;
	}

	// Toggle Method for boolean isChecked
	public void toggle()
	{
		isChecked = !isChecked;
	}

	// Resize the Background Image of this Object
	public void resizeBackground(Vector2 newSize)
	{
		buttonBackground.rectTransform.sizeDelta = newSize;
	}
}
