using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float damage;
    [SerializeField] private float speed;  // Velocidad de movimiento
    [SerializeField] private float frequency;
    [SerializeField] private int ValueDeath;
    private float currentSpeed;
    private float offset;
    private Point nextPoint;
    private Vector2 target;
    private Vector2 direction;
    private SpriteRenderer render;
    private Color originalColor;
    private Soldier currentEnemy;
    private Coroutine currentRutine;
    private bool attacking;
    private MoneyManager moneyManager;
    private List<Soldier> enemiesAttacking;
    [SerializeField] private Sprite[] listSpriteRenders;
    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = listSpriteRenders[Random.Range(0, listSpriteRenders.Length)];
        GetComponent<Animator>().SetBool("isRunning", true);
        enemiesAttacking = new List<Soldier>();
        moneyManager = GameObject.Find("scriptsGenerales").GetComponent<MoneyManager>();
        currentSpeed = speed;
        attacking = false;
        int option = Random.Range(0, 3);
        offset = (option - 1) * 0.5f; // -0.5 para izquierda, 0 para centro, 0.5 para derecha
        enabled = false;
        render = GetComponent<SpriteRenderer>();
        originalColor = render.color;
    }

    void Update()
    {
        // Mueve el enemigo hacia el objetivo (target) utilizando MoveTowards
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //Roto la imagen si la distancia entre el target y enemigo <=-0.1f y viceversa
        if((transform.position.x -target.x) <= 0)
            render.flipX = false;
        else
            render.flipX = true;

        // Comprueba si se ha llegado al punto de destino
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            // Cambia al siguiente punto y calcula el nuevo target
            nextPoint = nextPoint.NexPoint();

            if (nextPoint == null)
            {
                Destroy(gameObject); // Destruye al enemigo al final del recorrido
            }
            else
            {
                // Calcula la dirección hacia el siguiente punto
                direction = (nextPoint.GetPos() - (Vector2)transform.position).normalized;

                // Calcula el vector perpendicular a la dirección actual y aplica el offset
                Vector2 perpendicular = new Vector2(-direction.y, direction.x);
                target = nextPoint.GetPos() + perpendicular * offset;
            }
        }
    }

    public void setNextPoint(Point nextPoint)
    {
        this.nextPoint = nextPoint;
        enabled = true;

        // Calcula el primer target cuando se establece el primer punto
        direction = (nextPoint.GetPos() - (Vector2)transform.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        target = nextPoint.GetPos() + perpendicular * offset;
    }

    public float getVelocity() => currentSpeed;

    public Vector2 getNextDestination() => target;

    public void setLive(float cant)
    {
        life += cant;
        if(life <= 0)
        {
            moneyManager.setCantMoney(ValueDeath);
            enabled = false;
            StartCoroutine(waitingSecondsDeath());
        }
        else
            StartCoroutine(hit());
    }

    private IEnumerator hit()
    {
        render.material.color = Color.red;
        yield return new WaitForSeconds(.25f);
        render.material.color = originalColor;
    }

    public void move(Soldier currentEnemy,bool state)
    {
        if(!state)
        {
            if(this.currentEnemy == null)
            {
                GetComponent<Animator>().SetBool("isRunning",false);
                this.currentEnemy = currentEnemy;
            }
            
            enemiesAttacking.Add(currentEnemy);
        }
        else
        {
            GetComponent<Animator>().SetBool("isRunning", true);
            attacking = false;
            this.currentEnemy = null;
            
            if(attacking)
                StopCoroutine(currentRutine);
            deleteEnemies();
        }

        enabled = state;
    }
    private IEnumerator waitingSecondsDeath()
    {
        GetComponent<Animator>().SetBool("death", true);
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
    private void deleteEnemies() => enemiesAttacking.Clear();

    public void attack()
    {
        if(!attacking)
        {
            GetComponent<Animator>().SetBool("attacking", true);
            currentSpeed = 0;
            attacking = true;
            currentRutine = StartCoroutine(rutineAttack());
        }
    } 

    private IEnumerator rutineAttack()
    {
        while(attacking)
        {
            if(currentEnemy != null && currentEnemy.getLife() > 0)
            {
                currentEnemy.setLive(-damage);
                yield return new WaitForSeconds(frequency);
            }
            else
            {
                currentEnemy = null;
                break;
            }
        }

        List<Soldier> deleteSoldier = new List<Soldier>();

        foreach(Soldier s in enemiesAttacking) // si están muertos o están atacando a otros los borro
        {
            if(s == null || s.getLife() <= 0 || s.getCurrentEnemy() != gameObject)
                deleteSoldier.Add(s);
        }
    
        foreach(Soldier soldier in deleteSoldier)
            enemiesAttacking.Remove(soldier);

        // Si quedan enemigos atacando, selecciono el primero como nuevo target
        currentEnemy = enemiesAttacking.FirstOrDefault(soldier => soldier.getAttacking());
            
        if(currentEnemy == null)
        { 
            currentSpeed = speed;
            attacking = false;
            enabled = true;
            GetComponent<Animator>().SetBool("isRunning", true);
            GetComponent<Animator>().SetBool("attacking", false);
        }
        else
        StartCoroutine(checkForEnemies());
    }

    private IEnumerator checkForEnemies()
    {
        yield return new WaitForEndOfFrame();
        currentRutine = StartCoroutine(rutineAttack());
    }

    public float getLife() => life;
}
