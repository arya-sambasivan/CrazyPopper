using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popper : MonoBehaviour
{
    public PopperColor currentPopperColor;
    private GamePlayManager _gamePlayManager;
    [SerializeField] private Sprite purpleSpriteImage;
    [SerializeField] private Sprite BlueSpriteImage;
    [SerializeField] private Sprite YellowSpriteImage;
    [SerializeField] private AudioClip popAudioClip;
    [SerializeField] private AudioSource popAudiosource;
    private int totalNOofTaps;
    // Start is called before the first frame update
    void Start()
    {
        _gamePlayManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
        if(popAudiosource!=null)
        {
            popAudiosource.clip = popAudioClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatePopperSprite()
    {
        if(currentPopperColor==PopperColor.PURPLE)
        {
            this.transform.GetComponent<SpriteRenderer>().sprite = purpleSpriteImage;
        }
       else if (currentPopperColor == PopperColor.BLUE)
        {
            this.transform.GetComponent<SpriteRenderer>().sprite = BlueSpriteImage;
        }
       else if (currentPopperColor == PopperColor.YELLOW)
        {
            this.transform.GetComponent<SpriteRenderer>().sprite = YellowSpriteImage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        checkForReaction();
    }
    public void OnMouseDown()
    {
       totalNOofTaps = _gamePlayManager.updateTapBalance();
        if(totalNOofTaps>-1)
        {
            popAudiosource.Play();
            checkForReaction();
        }
        

    }
    private void checkForReaction()
    {
        
        _gamePlayManager.updateTotalColorValue();
        if(currentPopperColor == PopperColor.PURPLE)
        {
            
            _gamePlayManager.popperexplosion(this.gameObject);
        }
        else if (currentPopperColor == PopperColor.BLUE)
        {
            currentPopperColor = PopperColor.PURPLE;
            updatePopperSprite();
        }
        else if (currentPopperColor == PopperColor.YELLOW)
        {
            currentPopperColor = PopperColor.BLUE;
            updatePopperSprite();
        }

    }
}
