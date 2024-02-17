using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleFlappy : MonoBehaviour
{
    //Déclarations des variables (publiques)
    public float vitesseX;    //La vitesse de déplacement horizontal de Flappy
    public float vitesseY;    //La vitesse de déplacement vertical de Flappy

    // Fonction qui gère les déplacements et le saut de Flappy à l'aide des touches Right (ou A), Left (ou D) et Up (ou W).
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            vitesseX -= 1;
        } 
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            vitesseX += 1;
        }
        else 
        { 
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            vitesseY += 1;
        }
        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;
        }

        // On ajuste la vélocité du personnage en lui attribuant la valeur de la variable locale
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
    }
}
