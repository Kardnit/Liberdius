using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

public class MagicSystem : MonoBehaviour
{
	public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;

	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	private bool recognized;

	public PlayerStats playerStats;

	public Transform spellSpawnPoint;

	public GameObject manaSpell;

	void Start()
	{
		drawArea = new Rect(0, 0, Screen.width, Screen.height);

		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/");
		foreach (TextAsset gestureXml in gesturesXml)
		{
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		}
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		}

		if (drawArea.Contains(virtualKeyPosition))
		{
			if (Input.GetMouseButtonDown(0))
			{
				++strokeId;

				Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
				currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

				gestureLinesRenderer.Add(currentGestureLineRenderer);

				vertexCount = 0;
			}

			if (Input.GetMouseButton(0) && currentGestureLineRenderer != null)
			{
				points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

				if (currentGestureLineRenderer == null)
				{
					return;
				}

				currentGestureLineRenderer.positionCount = ++vertexCount;
				currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
			}
		}

		if (Input.GetMouseButtonDown(1))
		{
			RecognizeGesture();
			DeleteLines();
		}
	}

	void OnGUI()
	{
		GUI.color = new Color(0, 0, 0, 0);
		GUI.Box(drawArea, "");
	}

	void RecognizeGesture()
	{
		try
		{
			recognized = true;

			if (points.Count == 0 || trainingSet.Count == 0)
			{
				return;
			}

			Gesture candidate = new Gesture(points.ToArray());
			Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

			if (recognized)
			{

				if (gestureResult.GestureClass == "line")
				{
					if (playerStats.mana.value >= 20f)
					{
						playerStats.mana.Use(20f);
						CastManaSpell();
					}
				}
			}
		}
		catch (IndexOutOfRangeException)
		{

		}
		catch (Exception)
		{

		}
	}

	void DeleteLines()
	{
		recognized = false;
		strokeId = -1;

		points.Clear();

		foreach (LineRenderer lineRenderer in gestureLinesRenderer)
		{
			lineRenderer.positionCount = 0;
			Destroy(lineRenderer.gameObject);
		}

		gestureLinesRenderer.Clear();
	}

	void CastManaSpell()
	{
		if (manaSpell != null && spellSpawnPoint != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - spellSpawnPoint.position).normalized;

            GameObject instantiatedSpell = Instantiate(manaSpell, spellSpawnPoint.position, spellSpawnPoint.rotation);

            Rigidbody2D rb = instantiatedSpell.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(direction * 500f);
            }
        }
	}
}
