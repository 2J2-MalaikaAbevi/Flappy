using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleFlappy : MonoBehaviour
{
    //D�clarations des variables (publiques)
    public float vitesseX;    //La vitesse de d�placement horizontal de Flappy
    public float vitesseY;    //La vitesse de d�placement vertical de Flappy

    // Fonction qui g�re les d�placements et le saut de Flappy � l'aide des touches Right (ou A), Left (ou D) et Up (ou W).
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            vitesseX = -0.5f;
        } 
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            vitesseX = 0.5f;

        }
        else 
        { 
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            vitesseY = 3.5f;
        }
        else
        {
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;
        }

        // On ajuste la v�locit� du personnage en lui attribuant la valeur de la variable locale
        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);
    }
}
