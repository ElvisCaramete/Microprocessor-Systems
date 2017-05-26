using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSlideController : MonoBehaviour {

	//refrence for the pause menu panel in the hierarchy
	public GameObject cameraPanel;
	public GameObject humidityPanel;
	public GameObject moisturePanel;
	public GameObject temperaturePanel;
	public GameObject lightPanel;

	public GameObject mainMenuCanvas;
	//animator reference
	private Animator animCamera;
	private Animator animHumidity;
	private Animator animMoisture;
	private Animator animTemperature;
	private Animator animLight;
	private CanvasGroup canvasGroup;
	void Start () {
		canvasGroup = mainMenuCanvas.GetComponent<CanvasGroup> ();
		animCamera = cameraPanel.GetComponent<Animator>();
		animCamera.enabled = false;
		animHumidity = humidityPanel.GetComponent<Animator>();
		animHumidity.enabled = false;
		animMoisture = moisturePanel.GetComponent<Animator>();
		animMoisture.enabled = false;
		animTemperature = temperaturePanel.GetComponent<Animator>();
		animTemperature.enabled = false;
		animLight = lightPanel.GetComponent<Animator>();
		animLight.enabled = false;
	}

	// Update is called once per frame
	public void Update () {
	}

	//Animation for each panel
	public void CameraPanel(){
		StartCoroutine( DoFade ());
		animCamera.enabled = true;
		//play the Slidein animation
		animCamera.Play("CameraSlideIn");
	}
	public void HumidityPanel(){
		StartCoroutine( DoFade ());
		animHumidity.enabled = true;
		//play the Slidein animation
		animHumidity.Play("HumiditySlideIn");
	}
	public void MoisturePanel(){
		StartCoroutine( DoFade ());
		animMoisture.enabled = true;
		//play the Slidein animation
		animMoisture.Play("MoistureSlideIn");
	}
	public void TemperaturePanel(){
		StartCoroutine( DoFade ());
		animTemperature.enabled = true;
		//play the Slidein animation
		animTemperature.Play("TemperatureSlideIn");
	}
	public void LightPanel(){
		StartCoroutine( DoFade ());
		animLight.enabled = true;
		//play the Slidein animation
		animLight.Play("LightSlideIn");
	}

	//animation for fading out
	public void CameraPanelOut(){
		StartCoroutine (FadeOut ());
		animCamera.Play("CameraSlideOut");

	}
	public void HumidityPanelOut(){
		StartCoroutine (FadeOut ());
		animHumidity.Play("HumiditySlideOut");

	}
	public void MoisturePanelOut(){
		StartCoroutine (FadeOut ());
		animMoisture.Play("MoistureSlideOut");

	}
	public void TemperaturePanelOut(){
		StartCoroutine (FadeOut ());
		animTemperature.Play("TemperatureSlideOut");

	}
	public void LightPanelOut(){
		StartCoroutine (FadeOut ());
		animLight.Play("LightSlideOut");

	}

	IEnumerator DoFade(){
		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= Time.deltaTime ;
			yield return null;
		}
		canvasGroup.interactable = false;
		yield return null;

	}

	IEnumerator FadeOut(){
		while (canvasGroup.alpha < 1) {
			canvasGroup.alpha += Time.deltaTime ;
			yield return null;
		}
		canvasGroup.interactable = true;
		yield return null;
	}

}