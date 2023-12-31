using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityMaker : MonoBehaviour
{
    [SerializeField] TextAsset layout;
    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject buildingPrefab;
    // [SerializeField] GameObject semaphorePrefab;
    [SerializeField] int tileSize;

    // Start is called before the first frame update
    void Start()
    {
        MakeTiles(layout.text);
        Debug.Log("City successfully created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeTiles(string tiles)
    {
        int x = 0;
        // Mesa has y 0 at the bottom
        // To draw from the top, find the rows of the file
        // and move down
        // Remove the last enter, and one more to start at 0
        int y = tiles.Split('\n').Length - 2;
        Debug.Log(y);

        Vector3 position;
        GameObject tile;

        for (int i=0; i<tiles.Length; i++) {
            if (tiles[i] == '>' || tiles[i] == '<') {
                position = new Vector3(x * tileSize, 0, y * tileSize);
                tile = Instantiate(roadPrefab, position, Quaternion.Euler(0, 90, 0));
                tile.transform.parent = transform;
                tile.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); // Adjust the scale as needed
                // Set the scale of the instantiated prefab
                x += 1;
            } else if (tiles[i] == 'v' || tiles[i] == '^') {
                position = new Vector3(x * tileSize, 0, y * tileSize);
                tile = Instantiate(roadPrefab, position, Quaternion.identity);
                tile.transform.parent = transform;
                tile.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); // Adjust the scale as needed
                x += 1;
            // } else if (tiles[i] == 's') {
            //     position = new Vector3(x * tileSize, 0, y * tileSize);
            //     tile = Instantiate(roadPrefab, position, Quaternion.identity);
            //     tile.transform.parent = transform;
            //     tile.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); // Adjust the scale as needed
            //     tile = Instantiate(semaphorePrefab, position, Quaternion.Euler(0, 90, 0));
            //     tile.transform.parent = transform;
            //     x += 1;
            // } else if (tiles[i] == 'S') {
            //     position = new Vector3(x * tileSize, 0, y * tileSize);
            //     tile = Instantiate(roadPrefab, position, Quaternion.Euler(0, 90, 0));
            //     tile.transform.parent = transform;
            //     tile.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); // Adjust the scale as needed
            //     tile = Instantiate(semaphorePrefab, position, Quaternion.identity);
            //     tile.transform.parent = transform;
            //     x += 1;
            // Bucle for existente
            // ...

            } else if (tiles[i] == 's') {
                position = new Vector3(x * tileSize, 0, y * tileSize);
                tile = Instantiate(roadPrefab, position, Quaternion.identity);
                tile.transform.parent = transform;
                tile.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); // Ajustar la escala según sea necesario
                
                // Comprueba si el vecino izquierdo no es otro 'S' o 's' para instanciar el semáforo
                if (i > 0 && (tiles[i - 1] != 'S' && tiles[i - 1] != 's')) {
                    // Instancia el semáforo a la izquierda del tile actual
                    // tile = Instantiate(semaphorePrefab, position + new Vector3(-tileSize, 0, 0), Quaternion.Euler(0, 90, 0));
                    tile.transform.parent = transform;
                }
                
                x += 1;
            } else if (tiles[i] == 'S') {
                position = new Vector3(x * tileSize, 0, y * tileSize);
                tile = Instantiate(roadPrefab, position, Quaternion.Euler(0, 90, 0));
                tile.transform.parent = transform;
                tile.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f); // Ajustar la escala según sea necesario
                
                // Comprueba si el vecino izquierdo no es otro 'S' o 's' para instanciar el semáforo
                if (i > 0 && (tiles[i - 1] != 'S' && tiles[i - 1] != 's')) {
                    // Instancia el semáforo a la izquierda del tile actual
                    // tile = Instantiate(semaphorePrefab, position + new Vector3(-tileSize, 0, 0), Quaternion.identity);
                    tile.transform.parent = transform;
                }
                
                x += 1;
                       
            } else if (tiles[i] == 'D') {
                position = new Vector3(x * tileSize, 0, y * tileSize);
                tile = Instantiate(buildingPrefab, position, Quaternion.Euler(0, 90, 0));
                tile.GetComponent<Renderer>().materials[0].color = Color.red;
                tile.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                tile.transform.parent = transform;
                x += 1;
            } else if (tiles[i] == '#') {
                position = new Vector3(x * tileSize, 0, y * tileSize);
                tile = Instantiate(buildingPrefab, position, Quaternion.identity);
                // tile.transform.localScale = new Vector3(1, Random.Range(0.5f, 2.0f), 1);
                tile.transform.localScale = new Vector3(0.05f, Random.Range(0.05f, 0.1f), 0.05f);
                tile.transform.parent = transform;
                x += 1;
            } else if (tiles[i] == '\n') {
                x = 0;
                y -= 1;
            }
        }

    }
}
