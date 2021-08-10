using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayScript : MonoBehaviour
{
    public GameManager gameManager;
    Camera cam;
    public Rigidbody2D rb1;
    public Rigidbody2D rb2;
    public Rigidbody2D rb3;
    private Vector3 currentPoint;
    private Vector3 endpointCoin1;
    private Vector3 endpointCoin2;
    private Vector3 endpointCoin3;
    public Vector2 minPower;
    public Vector2 maxPower;
    private float power = 100f;
    private Vector3 endPoint;
    private Vector2 force;
    private Vector2 steadyState = new Vector2(0, 0);
    public LineRenderer lr;
    public LineRenderer lr1;
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Coin3;
    private int counter = 0;
    private Vector3 startPointCoin1;
    private Vector3 startPointCoin2;
    private Vector3 startPointCoin3;
    public int score;
    private Vector2 infintesimalVelocity = new Vector2(0.1f, 0.1f);
    private bool permission1 = true;
    private bool permission2 = true;
    private bool permission3=true;
    public GameObject gameOverPanel;
    private static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
            //is coplanar, and not parallel
            if (Mathf.Approximately(planarFactor, 0f) &&
                !Mathf.Approximately(crossVec1and2.sqrMagnitude, 0f))
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }       
    }
    private static bool IntersectionCheck(Vector3 Start1,Vector3 End1,Vector3 Start2,Vector3 Start3)
    {
            Vector3 intersection;
            Vector3 aDiff = Start1 - End1;
            Vector3 bDiff = Start2 - Start3;
        
            if (LineLineIntersection(out intersection, End1, aDiff, Start3, bDiff))
            {
                float aSqrMagnitude = aDiff.sqrMagnitude;
                float bSqrMagnitude = bDiff.sqrMagnitude;
                if ((intersection - End1).sqrMagnitude <= aSqrMagnitude
                     && (intersection - Start1).sqrMagnitude <= aSqrMagnitude
                     && (intersection - Start3).sqrMagnitude <= bSqrMagnitude
                     && (intersection - Start2).sqrMagnitude <= bSqrMagnitude)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        return false;
    }

    private void Turn(Rigidbody2D player,GameObject coinNr,Rigidbody2D previousPlayer, Rigidbody2D nextPlayer) 
    {
        player.bodyType = RigidbodyType2D.Dynamic;
        previousPlayer.bodyType = RigidbodyType2D.Static;
        nextPlayer.bodyType = RigidbodyType2D.Static;
        RenderLine(previousPlayer.transform.position, nextPlayer.transform.position, lr1);
        Vector3 startPoint = player.transform.position;
        if (Input.GetMouseButton(0))
        {
            currentPoint = startPoint * 2 -cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0;
            var difference = currentPoint - startPoint;
            var direction = difference.normalized;
            var distance = Mathf.Min(125f, difference.magnitude);
            var endPosition = startPoint + direction * distance;
            RenderLine(startPoint, endPosition, lr);

        }

        if (Input.GetMouseButtonUp(0))
        {
                if (coinNr == Coin1) { counter = 1; }
                if (coinNr == Coin2) { counter = 2; }
                if (coinNr == Coin3) { counter = 0; }
                EndLine(lr);
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 0;
                if (player.velocity == steadyState)
                {
                    force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
                    player.AddForce(force * power, ForceMode2D.Impulse);
                }
        }       
    }

    public void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void RenderLine(Vector3 startPoint, Vector3 endPoint, LineRenderer linerender)
    {
        linerender.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;
        linerender.SetPositions(points);
    }
    public void EndLine(LineRenderer linerender)
    {
        linerender.positionCount = 0;
    }
    public void GameOver()
    {
        EndLine(lr1);
        gameOverPanel.SetActive(true);
    }
    public void Start()
    {
        cam = Camera.main;
        rb1 = Coin1.GetComponent<Rigidbody2D>();
        rb2 = Coin2.GetComponent<Rigidbody2D>();
        rb3 = Coin3.GetComponent<Rigidbody2D>();
        lr1 = Coin2.GetComponent<LineRenderer>();
    }

    public void Update()
    {
        if (counter  == 0)
        {
            endpointCoin3 = rb3.transform.position;
            startPointCoin1 = rb1.transform.position;
            if (score>0 && permission1 == true && IntersectionCheck(startPointCoin3, endpointCoin3, endpointCoin1, endpointCoin2)) 
            {
                score += 1;
                permission2 = true;
                permission1 = false;
            }
            if (rb3.velocity.magnitude < infintesimalVelocity.magnitude)
            {
                if (score % 3 != 0)
                {
                    GameOver();
                }
                else
                {
                    Turn(rb1, Coin1, rb3, rb2);
                }
            }
        }
        if (counter  == 1)
        {
            endpointCoin1 = rb1.transform.position;
            startPointCoin2 = rb2.transform.position;
            if (permission2 == true && IntersectionCheck(startPointCoin1, endpointCoin1, endpointCoin2, endpointCoin3))
            {
                score += 1;
                permission3 = true;
                permission2 = false;
            }
            if (rb1.velocity.magnitude < infintesimalVelocity.magnitude)
            {
                if (score % 3 != 1)
                {
                    GameOver();
                }
                else
                {
                    Turn(rb2, Coin2, rb1, rb3);
                }
            }
        }
        if (counter  == 2)
        {
            endpointCoin2 = rb2.transform.position;
            startPointCoin3 = rb3.transform.position;
            if (permission3 == true && IntersectionCheck(startPointCoin2, endpointCoin2, endpointCoin1, endpointCoin3))
            {
                score += 1;
                permission1 = true;
                permission3 = false;
            }
            if (rb2.velocity.magnitude < infintesimalVelocity.magnitude)
            {
                if (score % 3 != 2)
                {
                    GameOver();
                }
                else
                {
                    Turn(rb3, Coin3, rb2, rb1);
                }
            }
        }
    }
}