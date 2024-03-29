using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements horizontaux et du saut de Flappy � l'aide des touches : Left (ou A), Right (ou D) et Up (ou W).
   Gestion des d�tections des collisions entre le personnage et les objets du jeu.
   Gestion des sons
   Gestion de la fin de la partie
   Par : Mala�ka Abevi
   Derni�re modification : 10/03/2024
*/

public class ControleFlappy : MonoBehaviour
{
    //D�clarations des variables (publiques)
    public float vitesseX;    //La vitesse de d�placement horizontal de Flappy
    public float vitesseY;    //La vitesse de d�placement vertical de Flappy

    public Sprite flappyMalade; //L'image de Flappy malade
    public Sprite flappyBase; //L'image de Flappy de base
    public Sprite flappyMaladeVol; //L'image de Flappy malade quand il vole
    public Sprite flappyBaseVol;   //L'image de FLappy de base quand il vole

    public GameObject laPieceOr;  //Le game object de la pi�ce d'or
    public GameObject lePackVie;  //Le game object du pack de vie
    public GameObject leChampignon;  //Le game object du champignon
    public GameObject laGrille; //Le game object de la grille 

    public AudioClip sonCol;  //Le son de la collision avec une colonne
    public AudioClip sonOr;  //Le son de la collision avec la pi�ce d'or
    public AudioClip sonPack;  //Le son de la collision avec le pack de vie
    public AudioClip sonChamp;  //Le son de la collision avec le chamnpignon
    public AudioClip sonFinPartie;  //Le son de la fin de la partie

    bool flappyBlesse = false;  //La variable bool�enne pour le statut de Flappy, bless� ou non 
    bool partieTerminee = false;  //La variable bool�enne pour le statut de la partie, termin�e ou non

    public TextMeshProUGUI pointage;  //Le texte pour le pointage
    public TextMeshProUGUI messageFinPartie; //Le texte pour le message de fin de partie

    int leScore; //Variable pour le calcul du pointage

    Color transparenceMessageFin; //Variable pour la gestion de la couleur du texte de fin 

    // Fonction qui g�re les d�placements et le saut de Flappy � l'aide des touches Left (ou A), Right (ou D) et Up (ou W).
    void Update()
    {
        //Les touches peuvent �tre utilis�es tant que la partie n'est pas termin�e
        if (!partieTerminee)
        { 
            //On ajuste la variable vitesseX si la touche Left (ou A) ou Right (ou D) est appuy�e. (avec GetKey, on peut maintenir la touche enfonc� et Flappy avance/recule)
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

            //On ajuste la variable vitesseY si la touche Up (ou W) est appuy�e. (avec GetKeyDown, il faut r�appuyer sur la touche pour que Flappy saute d'avantage)
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                vitesseY = 3.5f;

                //On change l'image de Flappy pour qu'il aille l'air de battre des ailes
                //S'il est en sant�
                if (!flappyBlesse)
                {
                    GetComponent<SpriteRenderer>().sprite = flappyBaseVol;
                    //Test fonctionnement print(2);
                }
                //S'il est bless�
                else
                {
                    GetComponent<SpriteRenderer>().sprite = flappyMaladeVol;
                }
            }
            //Si les touches pour sauter sont relach�es, on redonne l'image avec les ailes lev�es, d�pendament s'il est bless� ou non
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                //S'il est en sant�
                if (!flappyBlesse)
                {
                    GetComponent<SpriteRenderer>().sprite = flappyBase;
                    //Test fonctionnement print(222);
                }
                //S'il est bless�
                else
                {
                    GetComponent<SpriteRenderer>().sprite = flappyMalade;
                }
            }
            //On ajuste la variable vitesseY avec la v�locit� en Y pour faire un arr�t plus naturel
            else
            {
                vitesseY = GetComponent<Rigidbody2D>().velocity.y;
            }

            
            // On ajuste la v�locit� du personnage en lui attribuant les valeurs des variables de vitesse X et Y
            GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);        
        }

        //On g�re le message de fin de la partie
        if (partieTerminee)
        {
            //On r�active le texte pour le message de fin 
            messageFinPartie.enabled = true;
            //On enregistre la couleur du texte dans la variable Color "transparence"
            transparenceMessageFin = messageFinPartie.color;
            //Puis, on modifie le niveau de transparence du texte de transparent � opaque graduellement
            transparenceMessageFin.a += 0.005f;
            //Puis on r�applique la nouvelle valeur de couleur
            messageFinPartie.color = transparenceMessageFin;

            //On augmente la taille de la police jusqu'� ce qu'elle atteigne 100
            if (messageFinPartie.fontSize < 100)
            {
                messageFinPartie.fontSize += 0.3f;
            }
        }

    }

    //Fonction pour la d�tection des collisions avec les objets et Flappy, puis des actions qui se produiront suite � une collision
    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        //On v�rifie si une collision est d�tect�e avec une colonne ou avec les extremit�s du d�cor
        if(infoCollision.gameObject.name == "ColonneHaut" || infoCollision.gameObject.name == "ColonneBas" || infoCollision.gameObject.name == "Decor")
        {
            //On soustrait 5 points dans la variable du score
            leScore -= 5;

            //On v�rifie si Flappy est en sant� lors de la collision
            //Si oui:
            if (!flappyBlesse)
            {
                //On donne alors une image de Flappy malade qui remplacera l'image de base de Flappy 
                GetComponent<SpriteRenderer>().sprite = flappyMalade;

                //On enregistre que Flappy est maintenant bless�e en rendant cette variable � true
                flappyBlesse = true;

                //print(flappyBlesse); (test dans la console)
            }
            else //Sinon, si Flappy est d�j� bless�e lors de la collision
            {
                //On enregistre que la partie est termin�e en rendant cette variable true
                partieTerminee = true;
                
                //Flappy peut maintenant tourner (en d�bloquant la contrainte de rotation Z)
                GetComponent<Rigidbody2D>().freezeRotation = false;

                //On augmente la vitesse angulaire de Flappy
                GetComponent<Rigidbody2D>().angularVelocity = 90f;

                //On d�sactive le collider de Flappy, donc il passe � travers tout
                GetComponent<Collider2D>().enabled = false;

                //On fait jouer le son de la fin de la partie et on r�duit le volume car le son de base est trop fort
                GetComponent<AudioSource>().PlayOneShot(sonFinPartie, 0.5f);

                //Puis, apr�s 5s, on appelle la fonction qui g�re le relancement de la partie
                Invoke("Rejouer", 5f);
                }

            //Son pour la collision avec la colonne et on r�duit le volume car le son de base est trop fort
            GetComponent<AudioSource>().PlayOneShot(sonCol, 0.5f);
        }

        //On v�rifie si un collision est d�tect�e avec la pi�ce d'or
        if(infoCollision.gameObject.name == "PieceOr")
        {
            //On additionne 5 points dans la variable du score
            leScore += 5;

            //On d�sactive alors la pi�ce d'or (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur al�atoire est choisit entre -2 et 2 pour repositionner la pi�ce d'or sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonn�es avec une position X fixe et la valeur al�atoire qui a �t� g�n�r�e
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Son pour la collision avec la pi�ce d'or et on r�duit le volume car le son de base est trop fort
            GetComponent<AudioSource>().PlayOneShot(sonOr, 0.5f);

            //On active l'animation de la grille
            laGrille.GetComponent<Animator>().enabled = true;

            //Apr�s 4 secondes, on appelle une fonction qui d�sactivera � nouveau l'animation de la grille
            Invoke("DesactiverAnimGrille", 4f);

            //Puis, on appelle une fonction qui servira a faire r�apparaitre la pi�ce d'or au bout de 6 secondes, le temps que la colonne sorte du d�cor
            Invoke("ReactiverPieceOr", 6f);
        }

        //On v�rifie si un collision est d�tect�e avec le pack de vie
        if (infoCollision.gameObject.name == "PackVie")
        {
            //On additionne 5 points dans la variable du score
            leScore += 5;

            //On enregistre que Flappy est maintenant en sant� en rendant cette variable � false
            flappyBlesse = false;

            /*On donne (ou redonne) alors une image de Flappy de base qui remplacera l'image de Flappy malade 
             (ou elle demeure la m�me si Flappy n'�tait pas malade puisque c'est la m�me image)*/
            GetComponent<SpriteRenderer>().sprite = flappyBase;

            //On d�sactive alors le pack de vie (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur al�atoire est choisit entre -2 et 2 pour repositionner le pack de vie sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonn�es avec une position X fixe et la valeur al�atoire qui a �t� g�n�r�e
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Son pour la collision avec le pack de vie
            GetComponent<AudioSource>().PlayOneShot(sonPack);

            //Puis, on appelle une fonction qui servira a faire r�apparaitre le pack de vie au bout de 6 secondes, le temps que la colonne sorte du d�cor
            Invoke("ReactiverPackVie", 6f);
        }

        //On v�rifie si un collision est d�tect�e avec le champignon
        if (infoCollision.gameObject.name == "Champignon")
        {
            //On additionne 10 points dans la variable du score
            leScore += 10;

            //On grossit la taille de Flappy de 50%
            transform.localScale *= 1.5f;

            //On d�sactive alors le champignon (le game object)
            infoCollision.gameObject.SetActive(false);

            //Une valeur al�atoire est choisit entre -2 et 2 pour repositionner le pack de vie sur la verticale
            float valeurAleatoireY = Random.Range(-2f, 2f);

            //On donne alors de nouvelles coordonn�es avec une position X fixe et la valeur al�atoire qui a �t� g�n�r�e
            infoCollision.gameObject.transform.localPosition = new Vector2(-4, valeurAleatoireY);

            //Son pour la collision avec le champignon et on r�duit le volume car le son de base est trop fort
            GetComponent<AudioSource>().PlayOneShot(sonChamp, 0.5f);

            /*Puis, on appelle une fonction qui servira a faire r�apparaitre le champignon et � rendre 
             la taille d'origine � Flappy au bout de 6 secondes, le temps que la colonne sorte du d�cor*/
            Invoke("ReactiverChampignon", 6f);
        }

        //On red�finit le texte du pointage avec la valeur de score enregist�e
        pointage.text = "POINTAGE : " + leScore.ToString();
    }

    //Fonction pour d�sactiver l'animation de la grille 
    void DesactiverAnimGrille()
    {
        //d�sactivation de l'animation de la grille
        laGrille.GetComponent<Animator>().enabled = false;
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

    //Fonction pour r�activer le champignon (game object) et redonner la taille d'origine � Flappy
    void ReactiverChampignon()
    {
        //R�activation du champignon
        leChampignon.SetActive(true);

        //On redonne la taille d'origine � Flappy en la diminuant de 50%
        transform.localScale /= 1.5f;
    }

    //Fonction pour rejouer une partie en relancant la sc�ne
    void Rejouer()
    {
        //Faire rejouer la bonne sc�ne
        SceneManager.LoadScene(4);
    }

}
