using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements horizontaux et du saut de Flappy à l'aide des touches : a ou flèche gauche, d ou flèche haut, w ou flèche droite.
   Gestion des détections de collisions entre le personnage et les objets du jeu.
   Par : Malaïka Abevi
   Dernière modification : 24/02/2024
*/

public class ControleFlappy : MonoBehaviour
{
    //Déclarations des variables (publiques)
    public float vitesseX;    //La vitesse de déplacement horizontal de Flappy
    public float vitesseY;    //La vitesse de déplacement vertical de Flappy

    public Sprite flappyMalade; //L'image de Flappy malade
    public Sprite flappyBase; //L'image de Flappy de base

    public GameObject laPieceOr;  //Le game object de la pièce d'or
    public GameObject lePackVie;  //Le game object du pack de vie
    public GameObject leChampignon;  //Le game object du champignon

    // Fonction qui gère les déplacements et le saut de Flappy à l'aide des touches Right (ou A), Left (ou D) et Up (ou W).
    void Update()
    {
        //On ajuste la variable vitesseX si la touche Right (ou A) ou Left (ou D) est appuyée
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            vitesseX = -0.5f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            vitesseX = 0.5f;

        }
        //On ajuste la variable vitesseX avec la vélocité en X pour faire un arret plus naturel
        else
        { 
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
        }

        //On ajuste la variable vitesseY si la touche Up (ou W) est appuyée
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            vitesseY = 3.5f;
        }
        //On ajuste la variable vitesseY avec la vélocité en Y pour faire un arret plus naturel
        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;
        }

        // On ajuste la vélocité du personnage en lui attribuant les valeurs des variables de vitesse X et Y
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
    }

    //Fonction pour la détection des collisions avec les objets et Flappy, puis des actions qui se produiront suite à une collision
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On vérifie si une collision est détectée avec une colonne
        if(infoCollision.gameObject.name == "ColonneHaut" || infoCollision.gameObject.name == "ColonneBas")
        {
            //On donne alors une image de Flappy malade qui remplacera l'image de base de Flappy 
            GetComponent<SpriteRenderer>().sprite = flappyMalade;
        }

        //On vérifie si un collision est détectée avec la pièce
        if(infoCollision.gameObject.name == "PieceOr")
        {    
            /*print(infoCollision.gameObject.name); Test pour vérifier la détection*/
            //On désactive alors la pièce d'or (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur aléatoire est choisit entre -2 et 2 pour repositionner la pièce d'or sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonnées avec une position X fixe et la valeur aléatoire qui a été générée
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Puis, on appelle une fonction qui servira a faire réapparaitre la pièce d'or au bout de 6 secondes, le temps que la colonne sorte du décor
            Invoke("ReactiverPieceOr", 6f);
        }

        //On vérifie si un collision est détectée avec le pack de vie
        if (infoCollision.gameObject.name == "PackVie")
        {
            /*On donne (ou redonne) alors une image de Flappy de base qui remplacera l'image de Flappy malade 
             (ou elle demeure la même si Flappy n'était pas malade puisque c'est la même image)*/
            GetComponent<SpriteRenderer>().sprite = flappyBase;

            //On désactive alors le pack de vie (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur aléatoire est choisit entre -2 et 2 pour repositionner le pack de vie sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonnées avec une position X fixe et la valeur aléatoire qui a été générée
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Puis, on appelle une fonction qui servira a faire réapparaitre le pack de vie au bout de 6 secondes, le temps que la colonne sorte du décor
            Invoke("ReactiverPackVie", 6f);
        }

        //On vérifie si un collision est détectée avec le champignon
        if (infoCollision.gameObject.name == "Champignon")
        {
            //On grossit la taille de Flappy de 50%
            transform.localScale *= 1.5f;

            //On désactive alors le champignon (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur aléatoire est choisit entre -2 et 2 pour repositionner le pack de vie sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonnées avec une position X fixe et la valeur aléatoire qui a été générée
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Puis, on appelle une fonction qui servira a faire réapparaitre le champignon au bout de 6 secondes, le temps que la colonne sorte du décor
            Invoke("ReactiverChampignon", 6f);
        }
    }

    //Fonction pour réactiver la pièce d'or (game object)
    void ReactiverPieceOr()
    {
        //Réactivation de la pièce d'or
        laPieceOr.SetActive(true);
    }

    //Fonction pour réactiver le pack de vie (game object)
    void ReactiverPackVie()
    {
        //Réactivation du pack de vie
        lePackVie.SetActive(true);
    }

    //Fonction pour réactiver le champignon (game object)
    void ReactiverChampignon()
    {
        //Réactivation du champignon
        leChampignon.SetActive(true);

        //On redonne la taille d'origine à Flappy en la diminuant de 50%
        transform.localScale /= 1.5f;
    }
}
