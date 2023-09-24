using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Enemy
{
    // The TreasureChest doesn't engage in any attacks, so IsAttacking is always false.
    public override bool IsAttacking => false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); // Call the Start method of the base class (Enemy).
    }

    // Update is called once per frame
    void Update()
    {

    }
}
