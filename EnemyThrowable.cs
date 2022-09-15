using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowable : Throwable
{
    public float duration;
    public float velocity;
    public float roundModifier;
    public float speedTotal = 1.5f;
    public Vector3 movement;
    public bool first = true;
    public bool simple = false;

    protected override void Start()
    {
        first = true;
        roundModifier = (1f + (.005f * GameManager.instance.completedWave));
        speedTotal = velocity * roundModifier * 1.05f;
        StartCoroutine(Duration());
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    protected override void Move(Vector3 input, float xSpeed = 1f, float ySpeed = 1f)
    {
        if (Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            // Reset MoveDelta
            moveDelta = new Vector3(input.x, input.y, 0);
            // See if we can move in this direction by casting a box there. If the box returns null, means we can move there
            transform.Translate(0, (float)(moveDelta.y * Time.deltaTime), 0);
            transform.Translate((float)(moveDelta.x * Time.deltaTime), 0, 0);
        }
    }

    public Vector3 GetStart()
    {
        Vector3 startPos = new Vector3(GameManager.instance.player.transform.position.x - transform.position.x,
                       GameManager.instance.player.transform.position.y - transform.position.y,
                       0).normalized;
        if (InterceptionDirection(GameManager.instance.player.transform.position, transform.position, GameManager.instance.player.GetVelocity(), speedTotal, result: out var direction))
        {
            startPos = direction * speedTotal;
        }
        return startPos;
    }


    public bool InterceptionDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result)
    {
        Vector2 aToB = b - a;
        float dC = aToB.magnitude;
        var alpha = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
        var sA = vA.magnitude;
        var r = sA / sB;
        if (MathClass.SolveQuadratic(a: 1 - r * r, b: 2 * r * dC * Mathf.Cos(alpha), c: -(dC * dC), out var root1, out var root2) == 0)
        {
            result = Vector2.zero;
            return false;
        }
        var dA = Mathf.Max(root1, root2);
        var t = dA / sB;
        var c = a + vA * t;
        result = (c - b).normalized;
        return true;
    }

    protected override void FixedUpdate()
    {

        if (!MainMenu.inMainMenu)
        {
            if (Pause.gameActive && SkillPause.gameActive)
            {
                if (first && !simple)
                {
                    movement = GetStart();
                    first = false;
                } 
                else if (first && simple)
                {
                    movement = new Vector3(1, 0, 0);
                    first = false;
                }
                this.Move(movement);
            }
        } 
        else
        {
            Destroy(gameObject);
        }
    }
}
