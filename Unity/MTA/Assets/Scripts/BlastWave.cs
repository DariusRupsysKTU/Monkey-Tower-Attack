using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    [SerializeField] private int pointsCount;
    [SerializeField] private float maxRadius;
    [SerializeField] private float speed;
    [SerializeField] private float startWidth;
    [SerializeField] private float blastForceMultiplier;
    [SerializeField] private int blastDamage;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointsCount + 1;
        lineRenderer.enabled = false;
    }

    public IEnumerator Blast()
    {
        lineRenderer.enabled = true;

        float currentRadius = 0f;

        while (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            Explode(currentRadius);
            yield return null;
        }
    }

    private void Explode(float currentRadius)
    {
        Collider2D[] inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, currentRadius);
    
        foreach (Collider2D coll in inExplosionRadius)
        {
            Rigidbody2D collRB = coll.GetComponent<Rigidbody2D>();
            if (collRB != null && collRB.transform.tag != "Currency")
            {
                Vector2 distanceVector = coll.transform.position - transform.position;
                if (distanceVector.magnitude > 0)
                {
                    float explosionForce = blastForceMultiplier / distanceVector.magnitude;
                    collRB.AddForce(distanceVector.normalized * explosionForce);

                    if (collRB.transform.tag == "Player")
                    {
                        collRB.GetComponent<PlayerHealth>().DamagePlayer(blastDamage, true);
                    }
                    if (collRB.transform.tag == "Enemy")
                    {
                        collRB.GetComponent<EnemyHealth>().DamageEnemy(0);
                    }
                }
            }
        }
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }
}
