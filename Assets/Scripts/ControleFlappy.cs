using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements horizontaux et du saut de Flappy à l'aide des touches : Left (ou A), Right (ou D) et Up (ou W).
   Gestion des détections des collisions entre le personnage et les objets du jeu.
   Gestion des sons
   Gestion de la fin de la partie
   Par : Malaïka Abevi
   Dernière modification : 02/03/2024
*/

public class ControleFlappy : MonoBehaviour
{
    //Déclarations des variables (publiques)
    public float vitesseX;    //La vitesse de déplacement horizontal de Flappy
    public float vitesseY;    //La vitesse de déplacement vertical de Flappy

    public Sprite flappyMalade; //L'image de Flappy malade
    public Sprite flappyBase; //L'image de Flappy de base
    public Sprite flappyMaladeVol; //L'image de Flappy malade quand il vole
    public Sprite flappyBaseVol;   //L'image de FLappy de base quand il vole

    public GameObject laPieceOr;  //Le game object de la pièce d'or
    public GameObject lePackVie;  //Le game object du pack de vie
    public GameObject leChampignon;  //Le game object du champignon

    public AudioClip sonCol;  //Le son de la collision avec une colonne
    public AudioClip sonOr;  //Le son de la collision avec la pièce d'or
    public AudioClip sonPack;  //Le son de la collision avec le pack de vie
    public AudioClip sonChamp;  //Le son de la collision avec le chamnpignon
    public AudioClip sonFinPartie;  //Le son de la fin de la partie

    bool flappyBlesse = false;  //La variable booléenne pour le statut de Flappy, blessé ou non 
    bool partieTerminee = false;  //La variable booléenne pour le statut de la partie, terminée ou non

    // Fonction qui gère les déplacements et le saut de Flappy à l'aide des touches Left (ou A), Right (ou D) et Up (ou W).
    void Update()
    {
        //Les touches peuvent être utilisées tant que la partie n'est pas terminée
        if (!partieTerminee)
        { 
            //On ajuste la variable vitesseX si la touche Left (ou A) ou Right (ou D) est appuyée. (avec GetKey, on peut maintenir la touche enfoncé et Flappy avance/recule)
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

            //On ajuste la variable vitesseY si la touche Up (ou W) est appuyée. (avec GetKeyDown, il faut réappuyer sur la touche pour que Flappy saute d'avantage)
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                vitesseY = 3.5f;

                //On change l'image de Flappy pour qu'il aille l'air de battre des ailes
                //S'il est en santé
                if (!flappyBlesse)
                {
                    GetComponent<SpriteRenderer>().sprite = flappyBaseVol;
                    print(2);
                }
                //S'il est blessé
                else
                {
                    GetComponent<SpriteRenderer>().sprite = flappyMaladeVol;
                }
            }
            //Si les touches pour sauter sont relachées, on redonne l'image avec les ailes levées, dépendament s'il est blessé ou non
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                //S'il est en santé
                if (!flappyBlesse)
                {
                    GetComponent<SpriteRenderer>().sprite = flappyBase;
                    print(222);
                }
                //S'il est blessé
                else
                {
                    GetComponent<SpriteRenderer>().sprite = flappyMalade;
                }
            }
            //On ajuste la variable vitesseY avec la vélocité en Y pour faire un arrêt plus naturel
            else
            {
                vitesseY = GetComponent<Rigidbody2D>().velocity.y;
            }

            
            // On ajuste la vélocité du personnage en lui attribuant les valeurs des variables de vitesse X et Y
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);        
        }
        
    }

    //Fonction pour la détection des collisions avec les objets et Flappy, puis des actions qui se produiront suite à une collision
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On vérifie si une collision est détectée avec une colonne
        if(infoCollision.gameObject.name == "ColonneHaut" || infoCollision.gameObject.name == "ColonneBas")
        {
            //On vérifie si Flappy est en santé lors de la collision
            //Si oui:
            if (!flappyBlesse)
            {
                //On donne alors une image de Flappy malade qui remplacera l'image de base de Flappy 
                GetComponent<SpriteRenderer>().sprite = flappyMalade;

                //On enregistre que Flappy est maintenant blessée en rendant cette variable à true
                flappyBlesse = true;

                //print(flappyBlesse); (test dans la console)
            }
            else //Sinon, si Flappy est déjà blessée lors de la collision
            {
                //On enregistre que la partie est terminée en rendant cette variable true
                partieTerminee = true;
                
                //Flappy peut maintenant tourner (en débloquant la contrainte de rotation Z)
                GetComponent<Rigidbody2D>().freezeRotation = false;

                //On augmente la vitesse angulaire de Flappy
                GetComponent<Rigidbody2D>().angularVelocity = 90f;

                //On désactive le collider de Flappy, donc il passe à travers tout
                GetComponent<Collider2D>().enabled = false;

                //On fait jouer le son de la fin de la partie
                GetComponent<AudioSource>().PlayOneShot(sonFinPartie);

                //Puis, après 5s, on appelle la fonction qui gère le relancement de la partie
                Invoke("Rejouer", 5f);
            }

            //Son pour la collision avec la colonne
            GetComponent<AudioSource>().PlayOneShot(sonCol);
        }

        //On vérifie si un collision est détectée avec la pièce d'or
        if(infoCollision.gameObject.name == "PieceOr")
        {
            //On désactive alors la pièce d'or (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur aléatoire est choisit entre -2 et 2 pour repositionner la pièce d'or sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonnées avec une position X fixe et la valeur aléatoire qui a été générée
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Son pour la collision avec la pièce d'or
            GetComponent<AudioSource>().PlayOneShot(sonOr);

            //Puis, on appelle une fonction qui servira a faire réapparaitre la pièce d'or au bout de 7 secondes, le temps que la colonne sorte du décor
            Invoke("ReactiverPieceOr", 7f);
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

            //Son pour la collision avec le pack de vie
            GetComponent<AudioSource>().PlayOneShot(sonPack);

            //Puis, on appelle une fonction qui servira a faire réapparaitre le pack de vie au bout de 7 secondes, le temps que la colonne sorte du décor
            Invoke("ReactiverPackVie", 7f);
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

            //Son pour la collision avec la pièce d'or
            GetComponent<AudioSource>().PlayOneShot(sonChamp);

            /*Puis, on appelle une fonction qui servira a faire réapparaitre le champignon et à rendre 
             la taille d'origine à Flappy au bout de 7 secondes, le temps que la colonne sorte du décor*/
            Invoke("ReactiverChampignon", 7f);
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

    //Fonction pour réactiver le champignon (game object) et rodonner la taille d'origine à Flappy
    void ReactiverChampignon()
    {
        //Réactivation du champignon
        leChampignon.SetActive(true);

        //On redonne la taille d'origine à Flappy en la diminuant de 50%
        transform.localScale /= 1.5f;
    }

    //Fonction pour rejouer une partie en relancant la scène
    void Rejouer()
    {
        SceneManager.LoadScene(3);
    }

}
