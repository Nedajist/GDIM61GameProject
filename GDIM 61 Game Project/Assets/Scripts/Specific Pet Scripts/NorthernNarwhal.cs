using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthernNarwhal : Pet
{
<<<<<<< Updated upstream
    [SerializeField] private float _secondsBetweenProjectiles = 3f;

    private void Update()
    {

    }


=======
    [SerializeField] GameObject aoePrefab;
    [SerializeField] float aoeCooldown = 3f;
    private bool triggerAOE = false;
>>>>>>> Stashed changes
    private bool isCharging = false;
    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }

    // Update is called once per frame
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Spearhead(isCharging);
    }
    void Spearhead(bool charge)
    {
        if (charge == false)
        {
            speed *= 10f;
            isCharging = true;
        }
        else
        {
            speed /= 10f;
            isCharging = false;
        }
    }

    void AOE()
    {
        Instantiate(aoePrefab, transform);
        aoePrefab.transform.localScale *= 2;
    }

    IEnumerator AOEManager()
    {
        yield return new WaitForSeconds(aoeCooldown);

        triggerAOE = true;
    }
}
