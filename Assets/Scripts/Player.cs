using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    public float GetHealthPct ()
    {
        return (float)currentHealth / maxHealth;
    }

    [SyncVar]
    public string username = "Loading...";

    public int kills;
    public int deaths;
    public Camera camera;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public void SetupPlayer ()
    {
        if (isLocalPlayer)
        {
            //Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup ()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients ()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }

    IEnumerator timer(float time){
        yield return new WaitForSeconds(time);
        camera.fieldOfView = 20; //Mathf.Lerp(60, 10, Time.deltaTime * 5);

    }



    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(timer(0.2f));

        }
        if(Input.GetKeyUp(KeyCode.E)){
            camera.fieldOfView = 60;// Mathf.Lerp(10, 60, Time.deltaTime * 5);
        }

    }
    [ClientRpc]
    public void RpcTakeDamage (int _amount, string _sourceID)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die(_sourceID);
        }
    }

    private void Die(string _sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
           // GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        deaths++;

        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        //Spawn a death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        //Switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned.");
    }

    public void SetDefaults ()
    {
        isDead = false;

        currentHealth = maxHealth;

        //Enable the components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //Enable the gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        //Enable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        //Create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

}






/*
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {
    private bool firstSetup;


    [SerializeField] private GameObject[] disableGameObjectsOnDeath;

    [SerializeField] private GameObject deathEffect;

    [SerializeField] private GameObject spawnEffect;


    [SyncVar]
    private bool _isDead = false;


    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    [SerializeField] private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;

    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        if(Input.GetKeyDown(KeyCode.K)){
            RpcTakeDamage(9999);
        }
    }

    private void Die()
    {
        isDead = true;
        //DISABLE COMPONENTS

        Debug.Log(transform.name + " is DEAD");

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }


        //Disable gameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }


        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;

        }


        //Create a spawn death effect
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        Debug.Log(transform.name + " IS DEAD.");

        if(isLocalPlayer){
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        //CALL RESPAWN
        StartCoroutine(Respawn());


    }

    private IEnumerator Respawn(){
        yield return new WaitForSeconds(GameManager.instance.matchsettings.respawntime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        Debug.Log(transform.name + " respawned.");
        yield return new WaitForSeconds(0.1f);
        //switch cameras

        SetupPlayer();
    }


    public void  SetupPlayer()
    {
        if(isLocalPlayer){
            //switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

        }



        CmdBroadCastNewPlayerSetup();
    }


    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetUpPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetUpPlayerOnAllClients()
    {

        if(firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];

            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;

        }

       

        SetDefaults();
    }

    public void SetDefaults(){
        isDead = false;
        currentHealth = maxHealth;

        //Set components active
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            Debug.Log(disableOnDeath[i]);
            Debug.Log(i);
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        //enable gameobjects

        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }


        Collider _col = GetComponent<Collider>();
        if(_col != null){
            _col.enabled = true;
        }
       

        //create spawn effects
        GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);


    }
    [ClientRpc]
    public void RpcTakeDamage(int _damage){
        if (isDead)
            return;
        currentHealth -= _damage;
        Debug.Log(transform.name + " now has " + currentHealth + " health");

        if(currentHealth<=0){
            Die();
        }

    }
}



*/