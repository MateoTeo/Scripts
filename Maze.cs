using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell // TWORZENIE KLASY CELA
    {
        
        public bool visited; // CZY ZOSTALA ODWIEDZONA 
        public GameObject north; //1. ZMIENNA OBIEKTU GRY POLNOC
        public GameObject east;//2. ZMIENNA OBIEKTU GRY WSCHOD
        public GameObject west;//3. ZMIENNA OBIEKTU GRY ZACHOD
        public GameObject south;//4. ZMIENNA OBIEKTU GRY POLUDNIE
    }


    // ZMIENNE

    
    public GameObject wall; // ZMIENNA OBIEKTU GRY SCIANA
    public float wallLength = 1.0f; //DLUGOSC SCIANY
    public int xSize = 5; //ROZMIAR SCIANY W OSI X
    public int ySize = 5; //ROZMIAR SCIANY W OSI Y
    private Vector3 initialPos; // NASZA POCZATKOWA POZYCJA
    private GameObject wallHolder; //UCHWYT NA SCIANE
    private Cell[] cells; // TABLICA CEL 
    private int currentCell = 0; // MOWI NAM W KTOREJ KOMORCE JEST TERAZ (CELI), MA OBECNIE WARTOSC 0 DLATEGO PONIEWAZ BEDZIE TO WARTOSC LOSOWA
    private int totalCells; // CALKOWITA LICZBA KOMOREK (CEL)
    private int visitedCells = 0; // ODWIEDZONE KOMORKI (CELE)
    private bool startedBuilding = false; // ROZPOCZECIE BUDOWY 
    private int currentNeighbour = 0; // WARTOSC TA JEST GENEROWANA LOSOWO
    private List<int> lastCells; // PRYWATNA LISTA LICZB CALKOWITYCH Z OSTATNICH KOMOREK (CEL) ==> nie mozna jej uzywac bez System.Collections.Generic 
    private int backingUp = 0; // TWORZENIE KOPII ZAPASOWEJ 
    private int wallToBreak = 0; // BURZENIE SCIANY


    void Start()
    {
        CreateWalls(); // WYWOŁUJEMY FUNKCJE DO TWORZENIA SCIAN
        

    }

    void CreateWalls() // FUNKCJA DO TWORZENIA SCIAN
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze"; // TRZYMA W SRODKU WSPOLNE RZECZY
        initialPos = new Vector3((-xSize / 2) + wallLength / 2, 0.0f, (-ySize / 2) + wallLength / 2); // POCZATKOWA POZYCJA
        Vector3 myPos = initialPos; // MYPOS PRZECHOWUJE POCZATKOWA POZYCJE Z INITIALPOS
        GameObject tempWall; // TYMCZASOWA ŚCIANA 

        // CZTERY PETLE DO STAWIANIA SCIAN

        for (int i = 0; i < ySize; i++) // DLA OSI X
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 0.0f, initialPos.z + (i * wallLength) - wallLength / 2); 
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject; 
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        for (int i = 0; i <= ySize; i++) // DLA OSI Y
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength),0.0f, initialPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject; // OBROCENIE SCIAN ZA POMOCA EULER
                tempWall.transform.parent = wallHolder.transform;

            }
        }
        CreateCells(); //PO STWORZENIU SCIAN, STWORZ CELE
    }

    
   
    void CreateCells() //FUNKCJA DO TWORZENIA CELL
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize; // ILOSC KOMOREK (CEL)
        GameObject[] allWalls; // TABLICA Z WSZYSTKIMI SCIANAMI
        int children = wallHolder.transform.childCount; // PRZEKSZTALCONA WARTOSC Z UCHWYTU SCIENNEGO
        allWalls = new GameObject[children];
        cells = new Cell[xSize * ySize];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;
        

        // POBIERA WSZYSTKIE DZIECI 
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject; // PRZECHOWUJE WARTOSC 
        }

        // SPRAW BY SCIANY BYLY CELAMI
        for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++)
        {
            cells[cellprocess] = new Cell();
            cells[cellprocess].east = allWalls[eastWestProcess];
            cells[cellprocess].south = allWalls[childProcess +(xSize+1)*ySize];
           
            if (termCount == xSize) 
            {
                eastWestProcess += 2;
                termCount = 0;
            }
            else
            {
                eastWestProcess++; 
            }
            
            termCount++;
            childProcess++;
            
            cells[cellprocess].west = allWalls[eastWestProcess];
            cells[cellprocess].north = allWalls[(childProcess + (xSize + 1) * ySize) + xSize-1];
        }
        CreateMaze(); // KIEDY WSZYSTKIE CELE ZOSTANA UTWORZONE TO URUCHAMIAMY FUNKCE TWORZENIA LABIRYNTU

    }


    void CreateMaze() // FUNKCJA DO TWORZENIA LABIRYNTU
    {
        while (visitedCells < totalCells) // POROWNANIE ODWIEDZONYCH KOMOREK Z WSZYSTKIMI KOMORKAMI
        {
            if (startedBuilding)
            {
                GiveMeNeighbour();
                if (cells[currentNeighbour].visited ==false && cells[currentCell].visited == true) // SPRAWDZAMY CZY KOMORKI ZOSTALY ODWIEDZONE
                {
                    BreakWall(); // BURZENIE SCIANY
                    cells[currentNeighbour].visited = true; 
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count>0) // JESLI ZNALEZLISMY SASIADA TO STWORZ  KOPIE ZAPASOWA 
                    {
                        backingUp = lastCells.Count - 1; 
                    }
                }
            }

            else
            {
                currentCell = Random.Range(0, totalCells); // PRZEJDZ DO BIEZACEJ KOMORKI (WARTOSC MINIMALNA ,  SUMA KOMOREK)
                cells[currentCell].visited = true; // KOMORKA ZOSTALA ODWIEDZONA 
                visitedCells++;
                startedBuilding = true;
            }

            
        }
        Debug.Log("Finished");
    }

    //FUNKCJA DO BURZENIA SCIAN
    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:
                Destroy(cells[currentCell].north); break;
            case 2:
                Destroy(cells[currentCell].east); break;
            case 3:
                Destroy(cells[currentCell].west); break;
            case 4:
                Destroy(cells[currentCell].south); break;
        }
    }

    void GiveMeNeighbour() // DAJE SASIADA 
    {        
        int length = 0; // ZAWIERA INFORMACJE ILU SADZIADOW ZNALEZLISMY
        int[] neighbours = new int[4]; // MAKSYMALNIE MOZEMY MIEC 4 SASIADOW
        int[] connectingWall = new int [4]; // LACZENIE SCIAN PRZY MAXSYAMLNIE 4 SASIADOW
        int check = 0;
        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;


        // PETLA TWORZACA ZACHODNIA SCIANE
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)  
        {
            if (cells[currentCell +1].visited ==false)
            {
                neighbours[length] = currentCell + 1;
                connectingWall[length] = 3; // LACZENIE SCIAN PRZY DLUGOSCI 3 SASIADOW
                length++;
            }
        }

        // PETLA TWORZACA WSHODNIA SCIANE
        if (currentCell -1 >= 0 && currentCell != check) 
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbours[length] = currentCell - 1;
                connectingWall[length] = 2; // LACZENIE SCIAN PRZY DLUGOSCI 2 SASIADOW
                length++;
            }
        }

        // PETLA TWORZACA POLNOCNA SCIANE
        if (currentCell + xSize < totalCells) 
        {
            if (cells[currentCell + xSize].visited == false) 
            {
                neighbours[length] = currentCell +xSize;
                connectingWall[length] = 1; // LACZENIE SCIAN PRZY DLUGOSCI 1 SASIADA
                length++;
            }
        }

        //PETLA TWORZACA POLUDNIOWA SCIANE
        if (currentCell - xSize >=0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbours[length] = currentCell - xSize;
                connectingWall[length] = 4; // LACZENIE SCIAN PRZY DLUGOSCI 4 SASIADOW
                length++;
            }
        }

        // SPRAWDZAMY CZY ZNALEZLISMY SASIADA
        if (length!=0) 
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbour = neighbours[theChosenOne];
            wallToBreak = connectingWall[theChosenOne];
        }

        else
        {
            if (backingUp>0) 
            {
                currentCell = lastCells[backingUp];  
                backingUp--;
            }

        }
        

    }

    public GameObject Floor = null;


  
    



    /* Jest tutaj sosowany algorytm DFS. Polega na tym ze po wybraniu losowej komórki 
    (powiedzmy ze jest ich 25 tak jak jest poniżej zostaly przedstawione cyfry).
    Zaczynamy od wybrania losowej komórki np 12 to musimy znalezc jej sąsiadów, wiec jest 4 sasiadow.
    Tymi sąsiadami są cyfry 17, 11, 13, 07. Bedac w komorce 12 wybieramy losowo do ktorej komórki 
    bedziemy sie przemieszczac i niszczymy sciane pomiedzy nimi.



                      -------------------
                      |  20 21 22 23 24 |
                      |  15 16 17 18 19 |
                      |  10 11 12 13 14 |
                      |  05 06 07 08 09 | 
                      |  00 01 02 03 04 |
                      -------------------
    */


}
