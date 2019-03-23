using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public GameObject scope;
    public GameObject crossHair;

    [SerializeField]
    RectTransform thrusterFuelFill;

    [SerializeField]
    RectTransform healthBarFill;

    [SerializeField]
    Text ammoText;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject scoreboard;

    private Player player;
    private PlayerController controller;
    private WeaponManager weaponManager;

    IEnumerator TurnOnScope(float time)
    {
        yield return new WaitForSeconds(time);
        scope.SetActive(true);
        crossHair.SetActive(false);

    }

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    void Start()
    {
        PauseMenu.isOn = false;
    }

    void Update()
    {
        SetFuelAmount(controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthPct());
        SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.E)){

            StartCoroutine(TurnOnScope(0.2f));
        }
        if(Input.GetKeyUp(KeyCode.E)){
            WaitForSecondsRealtime c = new WaitForSecondsRealtime(0.2f);
            TurnOffScope();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    private void TurnOffScope()
    {
        scope.SetActive(false);
        crossHair.SetActive(true);


        // throw new NotImplementedException();
    }


    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;

    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }

   

}

/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;
    private PlayerController controller;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] GameObject scoreBoard;

    private void Start()
    {
        PauseMenu.isOn = false;
    }

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    private void Update()
    {
        if(controller==null){
            Debug.Log("Not yet referenced");
            return;  
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        if(Input.GetKeyDown(KeyCode.S)){
            scoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            scoreBoard.SetActive(false);
        }


        SetFuelAmount(controller.GetThrusterFuelAmount());
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }
}
*/
