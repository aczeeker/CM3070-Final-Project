using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    // Define variables to control the size and layout of the level.
    Vector2 worldSize = new Vector2(4, 4);  // The size of the game world.
    Room[,] rooms;                           // 2D array to store rooms.
    List<Vector2> takenPositions = new List<Vector2>();  // List of occupied positions.
    public int gridSizeX, gridSizeY, numberOfRooms;  // Grid size and number of rooms.
    public GameObject mapSelector;   // Prefab for selecting map room types.
    public Transform mapRoot;        // Parent transform for map objects.
    public GameObject chest;         // Prefab for chest objects.
    public GameObject storage;       // Prefab for storage room objects.
    public GameObject shop;          // Prefab for shop room objects.
    public GameObject levelExit;     // Prefab for level exit objects.
    public float chestRoomChance;    // Chance to spawn a chest room.
    private bool safeRoomExists;     // Flag to track the existence of a safe room.

    // This method is called when the object is awakened.
    public void Awake()
    {
        // Initialize level parameters based on LevelInfo.
        numberOfRooms = LevelInfo.numRooms;
        gridSizeX = LevelInfo.gridSize;
        gridSizeY = LevelInfo.gridSize;
        worldSize = new Vector2(gridSizeX, gridSizeY);

        // Ensure the number of rooms does not exceed grid capacity.
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }

        // Instantiate the map selector object and initialize flags.
        mapSelector = Object.Instantiate(mapSelector);
        safeRoomExists = false;

        // Call methods to generate and set up the level.
        CreateRooms();
        SetRoomDoors();
        DrawMap();
    }

    void CreateRooms()
    {
        //setup
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);
        rooms[gridSizeX, gridSizeY].isStartingRoom = true;
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        // magic numbers
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        // add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            // grab new position
            checkPos = NewPosition();
            // test new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //finalize position
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
            takenPositions.Insert(0, checkPos);
        }

        int edgeLayer = 0;
        List<Room> edgeRooms = null;
        while (edgeRooms == null)
        {
            edgeRooms = getEdgeRooms(edgeLayer);
            edgeLayer++;
        }
        // set index that not the 
        int endingRoom = Random.Range(0, edgeRooms.Count - 1);
        edgeRooms[endingRoom].setEnding();
    }

    public Room[,] getRooms()
    {
        return rooms;
    }

    // 0 = Outermost layer.
    List<Room> getEdgeRooms(int edgeLayer)
    {
        List<Room> toReturn = new List<Room>();
        // Top row
        for (int i = edgeLayer; i < rooms.GetLength(1) - edgeLayer; i++)
        {
            if (rooms[edgeLayer, i] != null)
            {
                toReturn.Add(rooms[edgeLayer, i]);
            }
        }
        // Middle rows: left and right column slots
        for (int i = edgeLayer + 1; i < rooms.GetLength(0) - edgeLayer - 1; i++)
        {
            if (rooms[i, edgeLayer] != null)
            {
                toReturn.Add(rooms[i, edgeLayer]);
            }
            if (rooms[i, rooms.GetLength(1) - edgeLayer - 1] != null)
            {
                toReturn.Add(rooms[i, rooms.GetLength(1) - edgeLayer - 1]);
            }
        }
        // Bottom row
        for (int i = edgeLayer; i < rooms.GetLength(1) - edgeLayer; i++)
        {
            if (rooms[rooms.GetLength(0) - edgeLayer - 1, i] != null)
            {
                toReturn.Add(rooms[rooms.GetLength(0) - edgeLayer - 1, i]);
            }
        }
        return toReturn;
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
            x = (int)takenPositions[index].x; //capture its x, y position
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f); //randomly pick where to look on hor or vert axis
            bool positive = (Random.value < 0.5f); //pick whether to be positive or negative on that axis
            if (UpDown)
            { //find the position based on the above bools
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
        return checkingPos;
    }
    // Generate a new position for a room with fewer neighbors.
    Vector2 SelectiveNewPosition()
    { // method differs from the above in the two commented ways
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                //instead of getting a room to find an adject empty space, we start with one that only 
                //as one neighbor. This will make it more likely that it returns a room that branches out
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
        { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
            print("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }
    // Calculate the number of neighbors a position has.
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }
    // Draw the map using the generated rooms.
    void DrawMap()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue; //skip where there is no room
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 22;//aspect ratio of map sprite
            drawPos.y *= 12;
            drawPos.y -= 0.5f;
            //create map obj and assign its variables
            MapSelector mapper = mapSelector.GetComponent<MapSelector>();
            mapper.type = room.type;
            mapper.up = room.doorTop;
            mapper.down = room.doorBot;
            mapper.right = room.doorRight;
            mapper.left = room.doorLeft;
            mapper.gameObject.transform.parent = mapRoot;
            mapper.PickRoom();
            GameObject roomObj = (GameObject) Object.Instantiate(mapper.getRoom(), drawPos, Quaternion.identity);
            CameraTriggerEnd centerTrigger = roomObj.transform.Find("Center").GetComponent<CameraTriggerEnd>();
            centerTrigger.isStartingRoom = room.isStartingRoom;
            if (room.isEndingRoom)
            {
                centerTrigger.isSafeRoom = true;
                GameObject exit = (GameObject)Object.Instantiate(levelExit, drawPos, Quaternion.identity);
                exit.transform.SetParent(roomObj.transform);
                room.cameraPosition = roomObj.transform.position;
                room.setRoom(roomObj);
                continue;
            }
            if (Random.value < 0.1 && !safeRoomExists && !room.isStartingRoom)
            {
                centerTrigger.isSafeRoom = true;
                GameObject s = (GameObject) Object.Instantiate(storage, drawPos, Quaternion.identity);
                GameObject s1 = (GameObject)Object.Instantiate(shop, drawPos + new Vector2(-7f, 3f), Quaternion.identity);
                s.transform.SetParent(roomObj.transform);
                s.transform.SetParent(roomObj.transform);
                safeRoomExists = true;
            } else if (Random.value < chestRoomChance && !room.isStartingRoom)
            {
                centerTrigger.isSafeRoom = true;
                GameObject s = (GameObject)Object.Instantiate(chest, drawPos, Quaternion.identity);
                s.transform.SetParent(roomObj.transform);
            } else
            {
                centerTrigger.isSafeRoom = false;
            }
            room.cameraPosition = roomObj.transform.position;
            room.setRoom(roomObj);
        }
    }
    // Set the doors for each room based on its neighbors.
    void SetRoomDoors()
    {
        for (int x = 0; x < ((gridSizeX * 2)); x++)
        {
            for (int y = 0; y < ((gridSizeY * 2)); y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }
                Vector2 gridPosition = new Vector2(x, y);
                if (y - 1 < 0)
                { //check above
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }
                if (y + 1 >= gridSizeY * 2)
                { //check bellow
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }
                if (x - 1 < 0)
                { //check left
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }
                if (x + 1 >= gridSizeX * 2)
                { //check right
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }
    }
}
