﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Bullet;
    public Transform FirePos;
    public  Animator ani;
    public ParticleSystem muzzle;
    public AudioClip shot;
    public AudioClip rel;
    public Texture2D mg;
    AudioSource Audio;
    bool shootlife = true; // 총알을 쓸 수 있는 상태다
    int bulletNumber = 60; // 1탄창에 들어있는 총알 수 (나중에 바꾸든가)
   
    int bulletToT = 180; // 남은 탄약
    bool loadTIme = true;
    bool moosin = false;
    public static int reloadBullet;

    public GUISkin newSkin;

    void Start()
    {
        Audio = this.GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        ani = this.GetComponent<Animator>();
        reloadBullet = 30 - bulletNumber;//재장전하면 채워질 탄약 수 
        if (SceneManager.GetActiveScene().name == "boss" && bulletToT <= 0)
        {
            bulletToT = 600;
            moosin = true;
        }

    }
    public void Shootmg()
    {
        if (shootlife &&  bulletNumber > 0 && loadTIme)
        {
            Audio.clip = shot;
            ani.enabled = true;
            GameObject go = Instantiate(Bullet, FirePos.transform.position, FirePos.transform.rotation) as GameObject;

            bulletNumber = bulletNumber - 1;
            if (bulletNumber == 0) { ani.enabled = false; }
            Debug.Log("히힛 총알 발싸:" + bulletNumber + " / " + bulletToT);
            Audio.Play();
            ani.Play("MachineGin_shoot");
            muzzle.Play();
            // 잠깐 쉰다
            shootlife = false;
            Invoke("Sleep", 0.07f);

        }
      /*  else if (touch.phase == TouchPhase.Ended)
        {
            ani.enabled = false;
        }
        else if (touch.phase == TouchPhase.Began)
        {
            ani.enabled = true;
        }*/
    }

    public void ReloadMG()
    {
     
            if (bulletNumber < 30 && bulletToT > 0) // 탄창이 풀이 아니고 잔탄있을 경우
            {
                Audio.clip = rel;
                loadTIme = false;
                ani.Play("MachineGun_reload");
                if (bulletToT - reloadBullet >= 0) // 잔탄이 여유로울떄 
                {
                    ani.enabled = true;

                    Audio.Play();
                    /* 입력을 2초간 받지 못하게 해야함 장전 모션 씹힘*/
                    Invoke("SleepLoad", 2.0f);

                    Debug.Log("장전완료:" + bulletNumber + " / " + bulletToT);
                    bulletToT = bulletToT - reloadBullet;//재장전한 만큼 잔탄 감소
                    bulletNumber = bulletNumber + reloadBullet;// 탄창 채워짐 
                }
                else // 잔탄이 부족할때 
                {
                    ani.enabled = true;


                    Audio.Play();
                    /* 입력을 2초간 받지 못하게 해야함 장전 모션 씹힘 */
                    Invoke("SleepLoad", 2.0f);

                    Debug.Log("장전완료 풀충 ㄴㄴ:" + bulletNumber + " / " + bulletToT);
                    bulletNumber += bulletToT;
                    bulletToT = 0;

                }

            }
            else { Debug.Log("총알 없어"); } //탄창이 풀이거나 잔탄이 없을 경우 
        
    }
    void Sleep()
    {
        shootlife = true;
    }
    void SleepLoad()
    {
        loadTIme = true;
    }
    void OnGUI()
    {
        GUI.skin = newSkin;
        GUI.DrawTexture(new Rect(0, Screen.height - Screen.height * 0.2f, Screen.width * 0.2f, Screen.height * 0.2f), mg);
        GUI.Label(new Rect(0, 0, Screen.width * 0.1f+150, Screen.height * 0.2f), "기관총 : " + bulletNumber + " / " + bulletToT, "thumb");
        if (SceneManager.GetActiveScene().name == "boss" && moosin == true)
        {
            GUI.Label(new Rect(Screen.width / 2 - 30, 25, Screen.width * 0.2f - 300, Screen.height * 0.2f), "무신의 가호로 총알이 충전!", "thumb");
        }
    }
}
