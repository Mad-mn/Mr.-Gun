using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] private int _headShotPrice;

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if(bullet != null)
        {
            if (bullet.GetActive())
            {
                CanvasController._canvasController.AddCoins(_headShotPrice);
            }
        }
    }
}
