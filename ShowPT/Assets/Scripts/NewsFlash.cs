using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsFlash : MonoBehaviour {

	[SerializeField]
	Camera myCamera;

	RectTransform rectTransform;
	Text myText;
	Vector3 initialTextPosition;
	float textSpeed = 60f;

	bool incomingNews = false;
	bool timeToMove = false;
	float moveStartTime;
	float speed = 50f;

	int secondsToNextHeadline;
	float secondsSinceLastHeadline;

	GameObject container;
	Vector3 positionOnScreen;
	Vector3 positionOffScreen;
	float distanceBetweenSpots;

	// Use this for initialization
	void Start () {
		container = transform.parent.gameObject;
		positionOnScreen = container.transform.position;
		positionOffScreen = positionOnScreen - new Vector3 (0f, container.GetComponent<RectTransform> ().rect.height * 2f, 0f);
		container.transform.position = positionOffScreen;
		distanceBetweenSpots = Vector3.Distance (positionOnScreen, positionOffScreen);

		rectTransform = GetComponent<RectTransform> ();
		myText = GetComponent<Text> ();
		initialTextPosition = transform.position;

		GenerateNews ();
	}

	// Update is called once per frame
	void Update () {
		if (secondsSinceLastHeadline > secondsToNextHeadline && incomingNews == false) 
		{
			secondsSinceLastHeadline = 0;
			moveStartTime = Time.time;
			incomingNews = true;
			timeToMove = true;

			transform.position = initialTextPosition/*new Vector3 (Screen.width + rectTransform.rect.width / 2f, initialTextPosition.y, initialTextPosition.z)*/;
		}

		if (timeToMove == true)
		{
			MoveNewsBanner ();
		}

		if (incomingNews) {
			transform.Translate (Vector3.left * textSpeed * Time.deltaTime);

			if (rectTransform.position.x < -rectTransform.rect.width) 
			{
				moveStartTime = Time.time;
				incomingNews = false;
				timeToMove = true;
			}
		} 
		else 
		{
			secondsSinceLastHeadline += Time.deltaTime;
		}
	}

	void MoveNewsBanner()
	{
		if (incomingNews && container.transform.position != positionOnScreen) 
		{
			container.transform.position = Vector3.Lerp (positionOffScreen, positionOnScreen, (Time.time - moveStartTime) * speed / distanceBetweenSpots);
			if (container.transform.position == positionOnScreen) 
			{
				timeToMove = false;
			}
		} 
		else if (container.transform.position != positionOffScreen) 
		{
			container.transform.position = Vector3.Lerp (positionOnScreen, positionOffScreen, (Time.time - moveStartTime) * speed / distanceBetweenSpots);
			if (timeToMove == true && secondsSinceLastHeadline > 3) 
			{
				GenerateNews ();

				timeToMove = false;
				incomingNews = false;
			}
		}
	}

	void GenerateNews()
	{
		secondsToNextHeadline = Random.Range (1, 5) * 60;
		secondsSinceLastHeadline = 0;

		int newsIndex = Random.Range (0, 3);
		Headlines (newsIndex);
	}

	void Headlines(int index)
	{
		switch (index) 
		{
		case 0:
			myText.text = "Última hora: Nuevo estudio demuestra que consumir amplias cantidades de Starlight Cola no resulta perjudicial para la " +
			"salud a pesar de sus amplios contenidos en salfumán. Dicho estudio fue apoyado, financiado y realizado por personas internas " +
			"a Starlight Cola, quienes afirman que no existe ningún tipo de interés ni manipulación de datos detrás de su ejecución.";
			break;
		case 1:
			myText.text = "Última hora: El famoso luchador de wrestling Mr. Hecatombe ha ganado las elecciones presidenciales por mayoría " +
			"abrumadora. Expertos afirman que el resultado estaba decidido desde que dejó tetraplégicos a sus oponentes durante el debate " +
			"del 15 de Diciembre. Sus primeras palabras en el despacho presidencial fueron '¡Voy a sacudir los cimientos del universo, de " +
			"aquí hasta el espacio exterior! ¡¡OH SI!!'. Analistas siguen sin ponerse de acuerdo sobre que demonios significa eso.";
			break;
		case 2:
			myText.text = "Última hora: Se confirma que la hija del presidente fue secuestrada por las ratas del sistema del alcantarillado de la " +
			"ciudad de Brimstorm. Un equipo diplomático especializado se ha desplazado hacia el complejo subterráneo para negociar el " +
			"rescate. Expertos en verminología sospechan que hará falta queso. En grandes cantidades.";
			break;
		case 3:
			myText.text = "Última hora: El líder de la 'Iglesia del Azúcar Moreno' ha amenazado con bombardear la costa Norte del país con su " +
			"arsenal de Misiles Edulcorados. Dichos misiles son armas biológicas cargadas de nanopartículas de azucar concentrado que " +
			"pueden resultar extremadamente peligrosas al generar altísimos índices de diabetes. Varios equipos de comandos en furgonetas " +
			"negras han sido enviados para detener a los adoradores de la Diabestia.";
			break;
		default:
			myText.text = "TEST";
			break;
		}
	}
}
