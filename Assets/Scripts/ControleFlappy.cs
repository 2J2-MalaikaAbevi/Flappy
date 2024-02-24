using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements horizontaux et du saut de Flappy � l'aide des touches : a ou fl�che gauche, d ou fl�che haut, w ou fl�che droite.
   Gestion des d�tections de collisions entre le personnage et les objets du jeu.
   Par : Mala�ka Abevi
   Derni�re modification : 24/02/2024
*/

public class ControleFlappy : MonoBehaviour
{
    //D�clarations des variables (publiques)
    public float vitesseX;    //La vitesse de d�placement horizontal de Flappy
    public float vitesseY;    //La vitesse de d�placement vertical de Flappy

    public Sprite flappyMalade; //L'image de Flappy malade
    public Sprite flappyBase; //L'image de Flappy de base

    public GameObject laPieceOr;  //Le game object de la pi�ce d'or
    public GameObject lePackVie;  //Le game object du pack de vie
    public GameObject leChampignon;  //Le game object du champignon

    // Fonction qui g�re les d�placements et le saut de Flappy � l'aide des touches Right (ou A), Left (ou D) et Up (ou W).
    void Update()
    {
        //On ajuste la variable vitesseX si la touche Right (ou A) ou Left (ou D) est appuy�e
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            vitesseX = -0.5f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            vitesseX = 0.5f;

        }
        //On ajuste la variable vitesseX avec la v�locit� en X pour faire un arret plus naturel
        else
        { 
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
        }

        //On ajuste la variable vitesseY si la touche Up (ou W) est appuy�e
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            vitesseY = 3.5f;
        }
        //On ajuste la variable vitesseY avec la v�locit� en Y pour faire un arret plus naturel
        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;
        }

        // On ajuste la v�locit� du personnage en lui attribuant les valeurs des variables de vitesse X et Y
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
    }

    //Fonction pour la d�tection des collisions avec les objets et Flappy, puis des actions qui se produiront suite � une collision
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On v�rifie si une collision est d�tect�e avec une colonne
        if(infoCollision.gameObject.name == "ColonneHaut" || infoCollision.gameObject.name == "ColonneBas")
        {
            //On donne alors une image de Flappy malade qui remplacera l'image de base de Flappy 
            GetComponent<SpriteRenderer>().sprite = flappyMalade;
        }

        //On v�rifie si un collision est d�tect�e avec la pi�ce
        if(infoCollision.gameObject.name == "PieceOr")
        {    
            /*print(infoCollision.gameObject.name); Test pour v�rifier la d�tection*/
            //On d�sactive alors la pi�ce d'or (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur al�atoire est choisit entre -2 et 2 pour repositionner la pi�ce d'or sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonn�es avec une position X fixe et la valeur al�atoire qui a �t� g�n�r�e
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Puis, on appelle une fonction qui servira a faire r�apparaitre la pi�ce d'or au bout de 6 secondes, le temps que la colonne sorte du d�cor
            Invoke("ReactiverPieceOr", 6f);
        }

        //On v�rifie si un collision est d�tect�e avec le pack de vie
        if (infoCollision.gameObject.name == "PackVie")
        {
            /*On donne (ou redonne) alors une image de Flappy de base qui remplacera l'image de Flappy malade 
             (ou elle demeure la m�me si Flappy n'�tait pas malade puisque c'est la m�me image)*/
            GetComponent<SpriteRenderer>().sprite = flappyBase;

            //On d�sactive alors le pack de vie (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur al�atoire est choisit entre -2 et 2 pour repositionner le pack de vie sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonn�es avec une position X fixe et la valeur al�atoire qui a �t� g�n�r�e
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Puis, on appelle une fonction qui servira a faire r�apparaitre le pack de vie au bout de 6 secondes, le temps que la colonne sorte du d�cor
            Invoke("ReactiverPackVie", 6f);
        }

        //On v�rifie si un collision est d�tect�e avec le champignon
        if (infoCollision.gameObject.name == "Champignon")
        {
            //On grossit la taille de Flappy de 50%
            transform.localScale *= 1.5f;

            //On d�sactive alors le champignon (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur al�atoire est choisit entre -2 et 2 pour repositionner le pack de vie sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonn�es avec une position X fixe et la valeur al�atoire qui a �t� g�n�r�e
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Puis, on appelle une fonction qui servira a faire r�apparaitre le champignon au bout de 6 secondes, le temps que la colonne sorte du d�cor
            Invoke("ReactiverChampignon", 6f);
        }
    }

    //Fonction pour r�activer la pi�ce d'or (game object)
    void ReactiverPieceOr()
    {
        //R�activation de la pi�ce d'or
        laPieceOr.SetActive(true);
    }

    //Fonction pour r�activer le pack de vie (game object)
    void ReactiverPackVie()
    {
        //R�activation du pack de vie
        lePackVie.SetActive(true);
    }

    //Fonction pour r�activer le champignon (game object)
    void ReactiverChampignon()
    {
        //R�activation du champignon
        leChampignon.SetActive(true);

        //On redonne la taille d'origine � Flappy en la diminuant de 50%
        transform.localScale /= 1.5f;
    }
}
