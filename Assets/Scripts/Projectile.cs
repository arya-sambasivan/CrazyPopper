using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private int _speed = 8;
    public Vector3 projectiledirection;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
    }
    void Update()
    {
        transform.position += (projectiledirection*_speed*Time.deltaTime);
        Vector3 stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        if(transform.position.x <-(stageDimensions.x)|| transform.position.x > stageDimensions.x || transform.position.y > stageDimensions.y || transform.position.y < - (stageDimensions.y / 2))
        {
            Destroy(this.gameObject);
        }

    }
}
