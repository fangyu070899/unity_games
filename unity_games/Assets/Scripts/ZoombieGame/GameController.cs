﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZoombieGame
{
    public class GameController : MonoBehaviour
    {
        public GameObject zoombie0;
        public GameObject zoombie1;
        public GameObject zoombie2;
        public GameObject zoombie3;
        public GameObject zoombie4;
        public GameObject zoombie5;
        private List<GameObject> zoombies;
        private List<float> button_index;

        private GameObject canvas;
        private UI canvasUI;

        GameObject audioSource;
        audio audio_player;

        float lowest;
        float highest;
        float delta_zoombie = 400f;

        public int killed = 0;
        public int zoombies_num = 10;
        public float timeRemaining = 30f;
        private float startCountdown = 3f;
        private int score = 0;
        private bool isPause = false;
        private bool isEnd = false;

        // Start is called before the first frame update
        void Start()
        {
            canvas = GameObject.Find("Canvas");
            canvasUI = canvas.GetComponent<UI>();

            audioSource = GameObject.Find("AudioSource");
            audio_player = audioSource.GetComponent<audio>();

            button_index = canvasUI.countButtonIndex();
            lowest = button_index[3] + 400;
            highest = lowest + delta_zoombie * 9;

            setUp();
        }

        // Update is called once per frame
        void Update()
        {
            if(startCountdown > 0)
            {
                startCountdown -= Time.deltaTime;
                int seconds = Mathf.CeilToInt(startCountdown);
                canvasUI.changeStartImage(seconds);
            }
            else
            {
                if(!isPause)canvasUI.setBtnInteractable(true);
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    int seconds = Mathf.CeilToInt(timeRemaining);
                    canvasUI.updateTime(seconds);
                }
                else
                {
                    if (!isEnd)
                    {
                        isEnd = true;
                        timeRemaining = 0;
                        timeout();
                        Pause(true);
                        canvasUI.gameOver(score);
                        audio_player.play_score();
                    }
                }
            }
        }

        void timeout()
        {
            Debug.Log("time out");
            canvasUI.setBtnInteractable(false);
            Debug.Log(score);
        }

        public void Pause(bool boolean)
        {
            if (boolean)
            {
                isPause = true;
                Time.timeScale = 0;
            }
            else
            {
                isPause = false;
                Time.timeScale = 1;
            }
        }

        void setUp()
        {
            zoombies = new List<GameObject>();
            float tmp_y = lowest;
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(zoombie0, canvas.transform);
                obj.transform.position = new Vector3(random_x(), tmp_y, 0);
                obj.transform.SetAsFirstSibling();
                zoombies.Add(obj);
                tmp_y = tmp_y + delta_zoombie;
            }

            killed = 0;
            zoombies_num = 10;
            timeRemaining = 30f;
            startCountdown = 3f;
            score = 0;
            Pause(false);
            isEnd = false;

        }

       public void shoot()
        {
            killed += 1;
            deleteZoombie();
            moveZoombie();
            addZoombie();
            countScore();
        }

        public float getZoombie()
        {
            Transform transform = zoombies[0].GetComponent<Transform>();
            Vector3 currentPosition = transform.position;
            Debug.Log(currentPosition.x);
            return currentPosition.x;
        }

        void addZoombie()
        {
            GameObject obj;
            if (zoombies_num <= 20)
            {
                obj = Instantiate(zoombie0, canvas.transform);
            }
            else if (zoombies_num <= 50)
            {
                obj = Instantiate(zoombie1, canvas.transform);
            }
            else if (zoombies_num <= 90)
            {
                obj = Instantiate(zoombie2, canvas.transform);
            }
            else if (zoombies_num <= 140)
            {
                obj = Instantiate(zoombie2, canvas.transform);
            }
            else if (zoombies_num <= 200)
            {
                obj = Instantiate(zoombie2, canvas.transform);
            }
            else
            {
                obj = Instantiate(zoombie3, canvas.transform);
            }
            obj.transform.position = new Vector3(random_x(), highest, 0);
            obj.transform.SetAsFirstSibling();
            zoombies.Add(obj);
            zoombies_num += 1;
        }

        void deleteZoombie()
        {
            GameObject obj = zoombies[0];
            zoombies.RemoveAt(0);
            if (obj!=null)Destroy(obj);
        }

        void moveZoombie()
        {

            for (int i = 0; i < zoombies.Count; i++)
            {
                Transform transform = zoombies[i].GetComponent<Transform>();
                Vector3 currentPosition = transform.position;
                currentPosition.y -= delta_zoombie;
                transform.position = currentPosition;
            }
        }

        void countScore()
        {
            if (killed <= 20)
                score += 10;
            else if (killed <= 50)
                score += 100;
            else if (killed <= 90)
                score += 1000;
            else if (killed <= 140)
                score += 10000;
            else if (killed <= 200)
                score += 100000;
            else
                score += 1000000;
        }

        float random_x()
        {
            int randomInt = Random.Range(0, 3);
            switch (randomInt)
            {
                case 0:
                    return button_index[0];
                case 1:
                    return button_index[1];
                case 2:
                    return button_index[2];
                default:
                    return button_index[0];
            }
        }

        public List<float> getButtonIndex()
        {
            return button_index;
        }

    }
}

