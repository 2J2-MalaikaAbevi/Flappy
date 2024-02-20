using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements horizontaux et du saut de Flappy � l'aide des touches : a ou fl�che gauche, d ou fl�che haut, w ou fl�che droite.
   Gestion des d�tections de collisions entre le personnage et les objets du jeu.
   Par : Mala�ka Abevi
   Derni�re modification : 20/02/2024
*/

public class ControleFlappy : MonoBehaviour
{
    //D�clarations des variables (publiques)
    public float vitesseX;    //La vitesse de d�placement horizontal de Flappy
    public float vitesseY;    //La vitesse de d�placement vertical de Flappy

    public Sprite flappyMalade; //L'image de Flappy malade

    public GameObject laPieceOr;
    public GameObject lePackVie;
    public GameObject leChampignon;

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
