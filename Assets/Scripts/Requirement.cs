using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requirement : MonoBehaviour
{

    public List<int> listOfInt = new List<int>();
    public int[] arrayOfInt;
    public int[,] array2DOfInt;

    public int[][] jaggedArrayOfInt;

    // Start is called before the first frame update
    void Start()
    {
        listOfInt.Add(2);
        listOfInt.Add(3);
        listOfInt.Add(9);
        listOfInt.Add(210);

        TestArray();
        TestJaggedArray();


        /*
        listOfInt.RemoveAt(2);
        listOfInt.Sort();
        for (int i = 0; 1 < listOfInt.Count; i++)
        {
            Debug.Log(listOfInt[i]);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestJaggedArray()
    {
        jaggedArrayOfInt = new int[10][];

        for (int x = 0; x < jaggedArrayOfInt.Length; x++)
        {
            jaggedArrayOfInt[x] = new int[x + 1];
        }

        for (int x = 0; x < jaggedArrayOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArrayOfInt[x].Length; y++)
            {
                jaggedArrayOfInt[x][y] = 123;
            }
        }
    }

    void TestArray()
    {
        arrayOfInt = new int[10];
        arrayOfInt[0] = 123;

        array2DOfInt = new int[10, 5];
        array2DOfInt[5, 3] = 123;
    }

}
