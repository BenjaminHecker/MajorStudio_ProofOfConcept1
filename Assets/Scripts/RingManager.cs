using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RingMarker ringMarkerPrefab;
    [SerializeField] private SpriteRenderer firstRing;
    [SerializeField] private SpriteRenderer secondRing;
    [SerializeField] private SpriteRenderer thirdRing;

    [Space]
    [SerializeField] private int firstRingCapacity = 3;
    [SerializeField] private int secondRingCapacity = 4;
    [SerializeField] private int thirdRingCapacity = 5;

    [Space]
    [SerializeField] private float firstRingRadius;
    [SerializeField] private float secondRingRadius;
    [SerializeField] private float thirdRingRadius;

    [Space]
    [SerializeField] private float emptyRingSpinSpeed = 1f;
    [SerializeField] private float completedRingSpinSpeed = 4f;

    private List<RingMarker> firstRingMarkers = new List<RingMarker>();
    private List<RingMarker> secondRingMarkers = new List<RingMarker>();
    private List<RingMarker> thirdRingMarkers = new List<RingMarker>();

    private float firstRingAngle = 0f;
    private float secondRingAngle = 0f;
    private float thirdRingAngle = 0f;

    public bool FirstRingComplete { get { return firstRingMarkers.Count >= firstRingCapacity; } }
    public bool SecondRingComplete { get { return secondRingMarkers.Count >= secondRingCapacity; } }
    public bool ThirdRingComplete { get { return thirdRingMarkers.Count >= thirdRingCapacity; } }
    public int CompleteRingsCount
    {
        get
        {
            return (FirstRingComplete ? 1 : 0) + (SecondRingComplete ? 1 : 0) + (ThirdRingComplete ? 1 : 0);
        }
    }

    

    public void Update()
    {
        Color newFirstRingColor = firstRing.color;
        newFirstRingColor.a = Mathf.InverseLerp(0f, firstRingCapacity, firstRingMarkers.Count);
        firstRing.color = newFirstRingColor;

        Color newSecondRingColor = secondRing.color;
        newSecondRingColor.a = Mathf.InverseLerp(0f, secondRingCapacity, secondRingMarkers.Count);
        secondRing.color = newSecondRingColor;

        Color newThirdRingColor = thirdRing.color;
        newThirdRingColor.a = Mathf.InverseLerp(0f, thirdRingCapacity, thirdRingMarkers.Count);
        thirdRing.color = newThirdRingColor;
        
        if (FirstRingComplete)
        {
            foreach (RingMarker marker in firstRingMarkers)
            {
                marker.spinSpeed = completedRingSpinSpeed;
                marker.UpdateMarker(firstRingAngle);
            }

            firstRingAngle += completedRingSpinSpeed / firstRingRadius * Time.deltaTime;
        }
        else
        {
            foreach (RingMarker marker in firstRingMarkers)
            {
                marker.spinSpeed = emptyRingSpinSpeed;
                marker.UpdateMarker(firstRingAngle);
            }

            firstRingAngle += emptyRingSpinSpeed / firstRingRadius * Time.deltaTime;
        }

        if (SecondRingComplete)
        {
            foreach (RingMarker marker in secondRingMarkers)
            {
                marker.spinSpeed = completedRingSpinSpeed;
                marker.UpdateMarker(secondRingAngle);
            }

            secondRingAngle -= completedRingSpinSpeed / secondRingRadius * Time.deltaTime;
        }
        else
        {
            foreach (RingMarker marker in secondRingMarkers)
            {
                marker.spinSpeed = emptyRingSpinSpeed;
                marker.UpdateMarker(secondRingAngle);
            }

            secondRingAngle -= emptyRingSpinSpeed / secondRingRadius * Time.deltaTime;
        }

        if (ThirdRingComplete)
        {
            foreach (RingMarker marker in thirdRingMarkers)
            {
                marker.spinSpeed = completedRingSpinSpeed;
                marker.UpdateMarker(thirdRingAngle);
            }

            thirdRingAngle += completedRingSpinSpeed / thirdRingRadius * Time.deltaTime;
        }
        else
        {
            foreach (RingMarker marker in thirdRingMarkers)
            {
                marker.spinSpeed = emptyRingSpinSpeed;
                marker.UpdateMarker(thirdRingAngle);
            }

            thirdRingAngle += emptyRingSpinSpeed / thirdRingRadius * Time.deltaTime;
        }
    }

    public void AddRingMarker()
    {
        if (FirstRingComplete)
        {
            if (SecondRingComplete)
            {
                if (!ThirdRingComplete)
                {
                    RingMarker newMarker = Instantiate(ringMarkerPrefab, thirdRing.transform);
                    thirdRingMarkers.Add(newMarker);

                    newMarker.radius = thirdRingRadius;
                    newMarker.angleOffset = (float) thirdRingMarkers.Count / thirdRingCapacity * 2f * Mathf.PI;
                    newMarker.UpdateMarker(thirdRingAngle);
                }
            }
            else
            {
                RingMarker newMarker = Instantiate(ringMarkerPrefab, secondRing.transform);
                secondRingMarkers.Add(newMarker);

                newMarker.radius = secondRingRadius;
                newMarker.angleOffset = (float) secondRingMarkers.Count / secondRingCapacity * 2f * Mathf.PI;
                newMarker.UpdateMarker(secondRingAngle);
            }
        }
        else
        {
            RingMarker newMarker = Instantiate(ringMarkerPrefab, firstRing.transform);
            firstRingMarkers.Add(newMarker);

            newMarker.radius = firstRingRadius;
            newMarker.angleOffset = (float) firstRingMarkers.Count / firstRingCapacity * 2f * Mathf.PI;
            newMarker.UpdateMarker(firstRingAngle);
        }
    }

    public void ResetOuterRing()
    {
        if (ThirdRingComplete)
        {
            foreach (RingMarker marker in thirdRingMarkers)
                Destroy(marker.gameObject);
            thirdRingMarkers.Clear();
        }
        else if (SecondRingComplete)
        {
            foreach (RingMarker marker in secondRingMarkers)
                Destroy(marker.gameObject);
            secondRingMarkers.Clear();
        }
        else if (FirstRingComplete)
        {
            foreach (RingMarker marker in firstRingMarkers)
                Destroy(marker.gameObject);
            firstRingMarkers.Clear();
        }
    }

    public void RemoveRings(int count)
    {
        int removeCount = 0;

        if (removeCount < count && FirstRingComplete)
        {
            removeCount++;

            foreach (RingMarker marker in firstRingMarkers)
                Destroy(marker.gameObject);
            firstRingMarkers.Clear();
        }
        if (removeCount < count && SecondRingComplete)
        {
            removeCount++;

            foreach (RingMarker marker in secondRingMarkers)
                Destroy(marker.gameObject);
            secondRingMarkers.Clear();
        }
        if (removeCount < count && ThirdRingComplete)
        {
            removeCount++;

            foreach (RingMarker marker in thirdRingMarkers)
                Destroy(marker.gameObject);
            thirdRingMarkers.Clear();
        }
    }
}
