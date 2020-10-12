using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPoint : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private GameObject hpbar;
    [SerializeField] private Transform hpBarRoot;

    [Header("General Properties")]
    public string enemyName;
    public Gradient hpBarColor;
    public Color hpBarFollowColor;

    [Header("Float Properties")]
    [Range(-2.5f, 2.5f)]
    public float hpBarOffset = 0.0f;

    //private
    private const float hpBarDisplaySpeed = 0.09f;
    private const float hpBarDisplayTime = 1.5f;
    private const float hpBarLengthFormula = 1.26603870577f; // this is the best balance I found and not thinking to modifiy it anyway.
    private GameObject hpbarHandle;
    private Camera cam;
    private Transform mainCanvas;
    private Collider collider;
    private Enemy enemyScript;
    private float maxHP = 1.0f;

    void Awake()
    {
        // Reference initialization
        collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogWarning("Collider not found in this enemy! Hp bar won't be able to resize itself.");
        }
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;
        if (mainCanvas == null)
        {
            Debug.LogWarning("Canvas not found as a reference. Hp bar will not show");
        }
        enemyScript = GetComponent<Enemy>();
        if (enemyScript == null)
        {
            Debug.LogWarning("Enemy script reerene not found");
        }

        maxHP = enemyScript.EnemyStat.health;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        //rigidbody.velocity = Vector3.zero;
        if (hpbarHandle != null)
        {
            var canvas = hpbarHandle.GetComponent<CanvasProperties>();
            
            //update hp bar position
            Vector3 barPos = hpBarRoot.position;
            barPos.y += hpBarOffset;

            Vector3 viewPos = cam.WorldToViewportPoint(barPos);
        
            hpbarHandle.transform.position = cam.WorldToScreenPoint(barPos);
            // make sure it wont go out of camera screen border if this enemy is in screen
            if (IsObjectInSight(transform, cam) && canvas.GetAlpha() > 0f)
            {
                if (cam.WorldToViewportPoint(barPos).y >= 1f)
                {
                    float hpbarHeight = (hpbarHandle.transform.GetChild(0).GetComponent<RectTransform>().rect.height);
                    hpbarHandle.transform.position = new Vector2(hpbarHandle.transform.position.x, Screen.height - hpbarHeight);
                }
            }
            canvas.SetProgressorColor(0, hpBarColor, enemyScript.EnemyStat.health / maxHP);
            canvas.SetProgressor(0, enemyScript.EnemyStat.health / maxHP);
            canvas.SetProgressor(1, Mathf.MoveTowards(canvas.GetProgressor(1), enemyScript.EnemyStat.health / maxHP, 0.5f * Time.deltaTime));
        }
    }

    public void TakeDamage(float damage)
    {
        enemyScript.ReceiveDamage(damage);

        if (hpbarHandle == null)
        {
            //instiate prefab
            hpbarHandle = Instantiate(hpbar);
            hpbarHandle.transform.SetParent(mainCanvas, false);
            hpbarHandle.SetActive(true);
            //set position
            Vector3 barPos = transform.position;
            barPos.y += hpBarOffset;
            hpbarHandle.transform.position = cam.WorldToScreenPoint(barPos);
            hpbarHandle.GetComponent<CanvasProperties>().SetTextMesh(0, enemyName);
            hpbarHandle.GetComponent<CanvasProperties>().SetAlpha(0.85f, hpBarDisplaySpeed, hpBarDisplayTime);
            hpbarHandle.GetComponent<CanvasProperties>().SetProgressorColor(1, hpBarFollowColor);
            hpbarHandle.GetComponent<CanvasProperties>().SetScaleX(collider.bounds.size.x * hpBarLengthFormula);
        }
        else
        {
            hpbarHandle.GetComponent<CanvasProperties>().SetAlpha(0.85f, hpBarDisplaySpeed, hpBarDisplayTime);
        }
    }

    public float GetCurrentHitPoint()
    {
        return enemyScript.EnemyStat.health;
    }

    private bool IsObjectInSight(Transform transform, Camera cam)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1
            && viewPos.y >= 0 && viewPos.y <= 1
            && viewPos.z > 0)
        {
            return true;
        }
        return false;
    }
}
