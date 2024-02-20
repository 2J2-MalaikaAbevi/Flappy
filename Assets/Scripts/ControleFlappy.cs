using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements horizontaux et du saut de Flappy à l'aide des touches : a ou flèche gauche, d ou flèche haut, w ou flèche droite.
   Gestion des détections de collisions entre le personnage et les objets du jeu.
   Par : Malaïka Abevi
   Dernière modification : 20/02/2024
*/

public class ControleFlappy : MonoBehaviour
{
    //Déclarations des variables (publiques)
    public float vitesseX;    //La vitesse de déplacement horizontal de Flappy
    public float vitesseY;    //La vitesse de déplacement vertical de Flappy

    public Sprite flappyMalade; //L'image de Flappy malade

    public GameObject laPieceOr;
    public GameObject lePackVie;
    public GameObject leChampignon;

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

    void OnCollisionEnter2D(Collision2D infoCollision)
    {
        if(infoCollision.gameObject.name == "ColonneHaut" || infoCollision.gameObject.name == "ColonneBas")
        {
            GetComponent<SpriteRenderer>().sprite = flappyMalade;
        }

        if(infoCollision.gameObject.name == "PieceOr")
        {
            infoCollision.gameObject.SetActive(false);

            float valeurAleatoireY = Random.Range(-3, 3);

            infoCollision.gameObject.transform.position = new Vector2(6, valeurAleatoireY);

            Invoke("ReactiverPieceOr", 5f);
        }

        if(infoCollision.gameObject.name == "PackVie")
        {
            infoCollision.gameObject.SetActive(false);

            float valeurAleatoireY = Random.Range(-3, 3);

            infoCollision.gameObject.transform.position = new Vector2(6, valeurAleatoireY);

            Invoke("ReactiverPackVie", 5f);
        }

        if (infoCollision.gameObject.name == "Champignon")
        {
            infoCollision.gameObject.SetActive(false);

            float valeurAleatoireY = Random.Range(-3, 3);

            infoCollision.gameObject.transform.position = new Vector2(6, valeurAleatoireY);

            Invoke("ReactiverChampignon", 5f);
        }
    }

    void ReactiverPieceOr()
    {
        laPieceOr.SetActive(true);
    }

    void ReactiverPackVie()
    {
        lePackVie.SetActive(true);
    }

    void ReactiverChampignon()
    {
        leChampignon.SetActive(true);
    }
}
