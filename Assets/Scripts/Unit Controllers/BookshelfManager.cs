using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfManager : MonoBehaviour
{
    public bool hasActive;
    // Start is called before the first frame update
    void Start()
    {
        hasActive = false;
    }
    private static BookshelfManager _instance = null;

    // Start is called before the first frame update

    public static BookshelfManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this; // Establish this object as the Singleton

        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
