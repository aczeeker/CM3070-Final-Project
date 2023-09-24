using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    // Room prefabs for different configurations.
    public GameObject spU, spD, spR, spL,
            spUD, spLR, spUR, spUL, spDR, spDL,
            spULD, spULR, spUDR, spDLR, spULDR;

    // Booleans to indicate the presence of doors in different directions.
    public bool up, down, left, right;

    // Type of the room (0: normal, 1: enter).
    public int type;

    // Colors for normal and enter states.
    public Color normalColor, enterColor;

    // Color for the main room.
    Color mainColor;

    // Reference to the sprite renderer.
    SpriteRenderer rend;

    // The selected room to return.
    private GameObject roomToReturn;

    // Called when the object starts.
    void Start()
    {
        // Pick the appropriate room based on door configuration.
        PickRoom();
    }

    // Get the selected room.
    public GameObject getRoom()
    {
        return roomToReturn;
    }

    // Pick the room based on the door configuration.
    public void PickRoom()
    { 
        if (up)
        {
            if (down)
            {
                if (right)
                {
                    if (left)
                    {
                        roomToReturn = spULDR;
                    }
                    else
                    {
                        roomToReturn = spUDR;
                    }
                }
                else if (left)
                {
                    roomToReturn = spULD;
                }
                else
                {
                    roomToReturn = spUD;
                }
            }
            else
            {
                if (right)
                {
                    if (left)
                    {
                        roomToReturn = spULR;
                    }
                    else
                    {
                        roomToReturn = spUR;
                    }
                }
                else if (left)
                {
                    roomToReturn = spUL;
                }
                else
                {
                    roomToReturn = spU;
                }
            }
            return;
        }
        if (down)
        {
            if (right)
            {
                if (left)
                {
                    roomToReturn = spDLR;
                }
                else
                {
                    roomToReturn = spDR;
                }
            }
            else if (left)
            {
                roomToReturn = spDL;
            }
            else
            {
                roomToReturn = spD;
            }
            return;
        }
        if (right)
        {
            if (left)
            {
                roomToReturn = spLR;
            }
            else
            {
                roomToReturn = spR;
            }
        }
        else
        {
            roomToReturn = spL;
        }
    }
}
