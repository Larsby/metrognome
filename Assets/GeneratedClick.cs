using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GeneratedClick : MonoBehaviour
{
	public double gainBoost = 0.9;
	public double gainRelease = 0.99;
	public float BPM = 120;

	 
	public double frequency = 880;

	private double gain = 0.00;
	private double increment;
	private double phase;
	private double sampling_frequency = 44100;
	private int pos = 0;

	double clapCheck;
	bool canRecalculate = false;
	public Text BPMText;

	public Button increaseButton;
	public Button decreaseButton;
	public Button increaseButton10;
	public Button decreaseButton10;

	public Color myColor = Color.white;


	public Animator[] animations;
	int side = 0;

	void Start ()
	{
		sampling_frequency = AudioSettings.outputSampleRate;
		canRecalculate = true;
		recalculateTimes ();
		BPMText.text = "" + BPM;
	
	}

 
	void Update ()
	{
		recalculateTimes ();
	 
		increaseButton.image.color = new Color (myColor.r, myColor.g, myColor.b, increaseButton.image.color.a * 0.95f);
		increaseButton.GetComponentInChildren<Text> ().color = new Color (0, 0, 0, increaseButton.image.color.a);

		decreaseButton.image.color = new Color (myColor.r, myColor.g, myColor.b, decreaseButton.image.color.a * 0.95f);
		decreaseButton.GetComponentInChildren<Text> ().color = new Color (0, 0, 0, decreaseButton.image.color.a);

		increaseButton10.image.color = new Color (myColor.r, myColor.g, myColor.b, increaseButton10.image.color.a * 0.95f);
		increaseButton10.GetComponentInChildren<Text> ().color = new Color (0, 0, 0, increaseButton10.image.color.a);

		decreaseButton10.image.color = new Color (myColor.r, myColor.g, myColor.b, decreaseButton10.image.color.a * 0.95f);
		decreaseButton10.GetComponentInChildren<Text> ().color = new Color (0, 0, 0, decreaseButton10.image.color.a);

	}

	public void recalculateTimes ()
	{
		if (BPM < 45)
			BPM = 45;

		if (BPM > 250)
			BPM = 250;


		BPMText.text = "" + BPM;
			
		if (canRecalculate == true)
		{
			foreach (Animator a in animations)
			{
				a.speed = 1f / 120f * (BPM);
				if (side == 0)
					a.SetTime (0);
			}

			clapCheck = (sampling_frequency / BPM) * 60f;
			canRecalculate = false;
		}
	}

	public void increase1 ()
	{
		BPM++;
		BPMText.text = "" + BPM;
		increaseButton.image.color = myColor;

	}

	public void decrease1 ()
	{
		BPM--;
		BPMText.text = "" + BPM;
		decreaseButton.image.color = myColor;
	}

	public void increase10 ()
	{
		BPM += 10;
		BPMText.text = "" + BPM;
		increaseButton10.image.color = myColor;

	}

	public void decrease10 ()
	{
		BPM -= 10;
		BPMText.text = "" + BPM;
		decreaseButton10.image.color = myColor;
	}


	public void setBPM (float inValue)
	{
		BPM = (int)inValue;
		BPMText.text = "" + BPM;
	}


	void OnAudioFilterRead (float[] data, int channels)
	{


		// update increment in case frequency has changed
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{

			pos++;
			if (pos >= clapCheck)
			{
				gain = gainBoost;
				pos = 0;

				//recalcualte values
				canRecalculate = true;

				side++;
				if (side == 2)
					side = 0;

			}
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data [i] = (float)(gain * Math.Sin (phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2)
				data [i + 1] = data [i];
			if (phase > 2 * Math.PI)
				phase = 0;

			gain = gain * gainRelease;
		}
	}


	 

}


 