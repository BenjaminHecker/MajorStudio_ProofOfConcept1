using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer baseRing;
    [SerializeField] private SpriteRenderer firstRing;
    [SerializeField] private SpriteRenderer secondRing;
    [SerializeField] private SpriteRenderer thirdRing;

    [Space]
    [SerializeField] private RingMarker ringMarkerBasePrefab;
    [SerializeField] private RingMarker ringMarker1Prefab;
    [SerializeField] private RingMarker ringMarker2Prefab;
    [SerializeField] private RingMarker ringMarker3Prefab;

    [Space]
    [SerializeField] private int firstRingCapacity = 3;
    [SerializeField] private int secondRingCapacity = 4;
    [SerializeField] private int thirdRingCapacity = 5;

    [Space]
    [SerializeField] private float firstRingRadius;
    [SerializeField] private float secondRingRadius;
    [SerializeField] private float thirdRingRadius;

    [Space]
    [SerializeField] private Color emptyRingColor;
    [SerializeField] private Color completedRingColor;

    [Space]
    [SerializeField] private float emptyRingSpinSpeed = 1f;
    [SerializeField] private float completedRingSpinSpeed = 4f;

    [HideInInspector] public List<RingMarker> firstRingMarkers = new List<RingMarker>();
    [HideInInspector] public List<RingMarker> secondRingMarkers = new List<RingMarker>();
    [HideInInspector] public List<RingMarker> thirdRingMarkers = new List<RingMarker>();

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
        float firstRingRatio = Mathf.InverseLerp(0f, firstRingCapacity, firstRingMarkers.Count);
        float secondRingRatio = Mathf.InverseLerp(0f, secondRingCapacity, secondRingMarkers.Count);
        float thirdRingRatio = Mathf.InverseLerp(0f, thirdRingCapacity, thirdRingMarkers.Count);

        baseRing.color = Color.Lerp(emptyRingColor, completedRingColor, firstRingRatio);
        firstRing.color = baseRing.color;

        secondRing.color = Color.Lerp(emptyRingColor, completedRingColor, secondRingRatio);
        thirdRing.color = Color.Lerp(emptyRingColor, completedRingColor, thirdRingRatio);

        
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
                    RingMarker newMarker = Instantiate(ringMarker3Prefab, thirdRing.transform);
                    thirdRingMarkers.Add(newMarker);

                    newMarker.radius = thirdRingRadius;
                    newMarker.angleOffset = (float) thirdRingMarkers.Count / thirdRingCapacity * 2f * Mathf.PI;
                    newMarker.UpdateMarker(thirdRingAngle);

                    SoundManager.PlayMiscWithPitch("Chime 3", Mathf.Lerp(1.5f, 3f, (float)thirdRingMarkers.Count / thirdRingCapacity));
                }
            }
            else
            {
                RingMarker newMarker = Instantiate(ringMarker2Prefab, secondRing.transform);
                secondRingMarkers.Add(newMarker);

                newMarker.radius = secondRingRadius;
                newMarker.angleOffset = (float) secondRingMarkers.Count / secondRingCapacity * 2f * Mathf.PI;
                newMarker.UpdateMarker(secondRingAngle);

                SoundManager.PlayMiscWithPitch("Chime 3", Mathf.Lerp(1f, 2.5f, (float)secondRingMarkers.Count / secondRingCapacity));
            }
        }
        else
        {
            RingMarker newMarker = Instantiate(ringMarker1Prefab, firstRing.transform);
            firstRingMarkers.Add(newMarker);

            newMarker.radius = firstRingRadius;
            newMarker.angleOffset = (float) firstRingMarkers.Count / firstRingCapacity * 2f * Mathf.PI;
            newMarker.UpdateMarker(firstRingAngle);

            SoundManager.PlayMiscWithPitch("Chime 3", Mathf.Lerp(0.5f, 2f, (float)firstRingMarkers.Count / firstRingCapacity));
        }
    }

    public void ResetOuterRing()
    {
        if (thirdRingMarkers.Count > 0)
        {
            foreach (RingMarker marker in thirdRingMarkers)
                Destroy(marker.gameObject);
            thirdRingMarkers.Clear();
        }
        else if (secondRingMarkers.Count > 0)
        {
            foreach (RingMarker marker in secondRingMarkers)
                Destroy(marker.gameObject);
            secondRingMarkers.Clear();
        }
        else if (firstRingMarkers.Count > 0)
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
