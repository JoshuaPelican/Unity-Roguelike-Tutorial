using UnityEngine;

public class WFCTrackManager : MonoBehaviour
{

    public GameObject[] outerWallTiles;

    private OverlapWFC wfc;

    private void Awake()
    {
        wfc = GetComponent<OverlapWFC>();
        wfc.training = GameObject.FindWithTag("WFCTraining").GetComponent<Training>();
    }

    public void InitializeTrack()
    {
        wfc.Generate();
        wfc.Run();
        SetupWalls();
    }

    private void SetupWalls()
    {
        int columns = wfc.width;
        int rows = wfc.depth;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    GameObject chosenTile = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                    GameObject tileInstance = Instantiate(chosenTile, new Vector3(x, y, 0f), Quaternion.identity);

                    tileInstance.transform.SetParent(transform.GetChild(0).GetChild(0));
                }
            }
        }
    }
}
