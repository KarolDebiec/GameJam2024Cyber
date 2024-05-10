using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if(_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.6f);
        _collider.enabled = true;
    }
    private void SetPlayerOnPlatform(Collision2D other ,bool value)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if(player != null)
        {
            _playerOnPlatform = value;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision,true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, true);
    }
}
