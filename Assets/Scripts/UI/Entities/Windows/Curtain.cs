using System;
using UnityEngine;

namespace UI.Entities.Windows
{
    public class Curtain : Window
    {
        private static Curtain _instance;
        
        public static Curtain Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Curtain>();
                    
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}