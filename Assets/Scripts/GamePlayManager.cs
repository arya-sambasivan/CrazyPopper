using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum PopperColor
{
    PURPLE,
    BLUE,
    YELLOW
};

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject popperPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject PopperHolder;
    [SerializeField] private GameObject ProjectileHolder;
    [SerializeField] private GameObject PopperExplosionHolder;
    [SerializeField] private GameObject LevelUI;
    [SerializeField] private GameObject noOfTaplUI;
    [SerializeField] private int RowsCount = 5;
    [SerializeField] private int coloumnCount = 6;
    [SerializeField] private GameObject gameEndUI;
    private int level = 1;
    private int totalPopperColorValue = 0;
    private bool _gamePlayActive = false;
    [SerializeField] private AudioClip applauseAudiClip;
    [SerializeField] private AudioClip gameOverAudioClip;
    [SerializeField]  private AudioSource audiosource1;
    [SerializeField] private AudioSource audiosource2;
    private struct levelData
    {
        public int[,] popperPlacement;
        public int noOfTaps;
    };
    levelData popperleveldata;
    void Start()
    {
        LevelUI.GetComponent<Text>().text = "Level:  " + level;
        if(audiosource1!=null)
        {
            audiosource1.clip = applauseAudiClip;
        }
        if(audiosource2!=null)
        {
            audiosource2.clip = gameOverAudioClip;
        }
       gamePlaySreen();
    }
    private void Update()
    {
        if(_gamePlayActive==true)
        {
            if(popperleveldata.noOfTaps<=0 && totalPopperColorValue >=0 && ProjectileHolder.transform.childCount<=0 && PopperHolder.transform.childCount>0)
            {
                audiosource2.Play();
                _gamePlayActive = false;
                gameEndUI.SetActive(true);
               
            }
        }
    }
    private void gamePlaySreen()
    {
        updateLevelData();
        for (var i = 0; i < 6; i++)
        {
            for (var j = 0; j < 5; j++)
            {
                int colorCodeforPopper = popperleveldata.popperPlacement[j, i];
                if (colorCodeforPopper>0)
                {
                    totalPopperColorValue += colorCodeforPopper;
                    GameObject popperObj = Instantiate(popperPrefab);
                    if(colorCodeforPopper==1)
                    {
                        popperObj.GetComponent<Popper>().currentPopperColor = PopperColor.PURPLE;
                       
                    }
                    else if(colorCodeforPopper==2)
                    {
                        popperObj.GetComponent<Popper>().currentPopperColor = PopperColor.BLUE;
                    }
                    else if(colorCodeforPopper == 3)
                    {
                        popperObj.GetComponent<Popper>().currentPopperColor = PopperColor.YELLOW;
                    }
                    else
                    {
                        popperObj.GetComponent<Popper>().currentPopperColor = PopperColor.PURPLE;
                    }
                    popperObj.GetComponent<Popper>().updatePopperSprite();
                    float width = popperObj.transform.GetComponent<SpriteRenderer>().bounds.size.x;
                    float height = popperObj.transform.GetComponent<SpriteRenderer>().bounds.size.y;
                    Vector2 origin = new Vector2(0 - ((coloumnCount * width) / 2) - 2.4f, 0 + ((RowsCount * height) / 2) + 1);
                    popperObj.transform.position = origin + new Vector2(width * i * 1.8f, -(height * j * 1.5f));
                    popperObj.transform.parent = PopperHolder.transform;
                   
                }
               
            }

        }
        noOfTaplUI.GetComponent<Text>().text = "NoOfTap:  " + popperleveldata.noOfTaps;
        _gamePlayActive = true;
    }

    private void updateLevelData()
    {
        popperleveldata.popperPlacement = null;
        switch (level)
        {
            case 1:
                {
                    popperleveldata.popperPlacement = new int[5, 6] { { 1, 0, 0, 0, 1, 0 }, { 0, 1, 0, 0, 1, 0 }, { 0, 1, 0, 0, 0, 1 }, { 0, 0, 1, 0, 0, 1 }, { 0, 0, 1, 0, 0, 0 } };
                    popperleveldata.noOfTaps = 1;

                    break;
                }
            case 2:
                {
                    popperleveldata.popperPlacement = new int[5, 6] { { 0, 0, 0, 0, 0, 0 }, { 0, 1, 0, 0, 2, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 1, 0, 0, 3, 0 }, { 0, 0, 0, 0, 0, 0 } };
                    popperleveldata.noOfTaps = 3;

                    break;
                }
            case 3:
                {
                    popperleveldata.popperPlacement = new int[5, 6] { { 0, 1, 0, 1, 0, 1 }, { 0, 0, 0, 2, 0, 0 }, { 0, 0, 0, 1, 0, 1 }, { 0, 0, 0, 2, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };
                    popperleveldata.noOfTaps = 2;

                    break;
                }
        }

    }

    public int updateTapBalance()
    {
        popperleveldata.noOfTaps--;
        if(popperleveldata.noOfTaps>-1)
        {
            noOfTaplUI.GetComponent<Text>().text = "NoOfTap:  " + popperleveldata.noOfTaps;
        }
        return popperleveldata.noOfTaps;
    }

    public void popperexplosion(GameObject expObj)
    {
        Vector3 position = expObj.transform.position;
        GameObject projectileObj = Instantiate(projectilePrefab, position + new Vector3(0, 1, 0),Quaternion.identity);
        projectileObj.transform.GetComponent<Projectile>().projectiledirection = Vector3.up;
        projectileObj.transform.parent = ProjectileHolder.transform;
        projectileObj = Instantiate(projectilePrefab, position + new Vector3(0, -1, 0),Quaternion.identity);
        projectileObj.transform.GetComponent<Projectile>().projectiledirection = Vector3.down;
        projectileObj.transform.parent = ProjectileHolder.transform;
        projectileObj = Instantiate(projectilePrefab, position + new Vector3(1.2f, 0, 0),Quaternion.identity);
        projectileObj.transform.GetComponent<Projectile>().projectiledirection = Vector3.right;
        projectileObj.transform.parent = ProjectileHolder.transform;
        projectileObj = Instantiate(projectilePrefab, position + new Vector3(-1.2f, 0, 0),Quaternion.identity);
        projectileObj.transform.GetComponent<Projectile>().projectiledirection = Vector3.left;
        projectileObj.transform.parent = ProjectileHolder.transform;
        GameObject ExplosionObj = Instantiate(explosionPrefab,position, Quaternion.identity);
        ExplosionObj.transform.parent = PopperExplosionHolder.transform;
        Destroy(expObj);
        if(totalPopperColorValue<=0)
        {
            audiosource1.Play();
        }
        StartCoroutine("onAnimationCompletion", ExplosionObj);

    }

    private IEnumerator onAnimationCompletion(GameObject explodedObj)
    {
        
        yield return new WaitForSeconds(1);
        explodedObj.SetActive(false);
        Destroy(explodedObj);
        StopCoroutine("onAnimationCompletion");
        if (totalPopperColorValue <= 0)
        {
            level = level + 1;
            clearScreen();
            
        }
    }
    
    public void updateTotalColorValue()
    {
        totalPopperColorValue--;
    }

    private void clearScreen()
    {
        foreach (Transform child in PopperExplosionHolder.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ProjectileHolder.transform)
        {
            Destroy(child.gameObject);
        }
        LevelUI.GetComponent<Text>().text = "Level:  " + level;
        if(level>=4)
        {
            gameEndUI.SetActive(true);
            gameEndUI.transform.Find("GameOver").transform.GetComponent<Text>().text = "SOON";
            
        }
        else
        {
            gamePlaySreen();
        }
        
    }

}
