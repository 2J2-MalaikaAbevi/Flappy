using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilité générale du script:
   Gestion des déplacements horizontaux et des positions verticales des objets du jeu.
   Par : Malaïka Abevi
   Dernière modification : 23/02/2024
*/

public class DeplacementObjets : MonoBehaviour
{
    //Déclarations des variables (publiques)
    public float vitesse; //La vitesse de l'objet
    public float positionFin; //La position de fin de l'objet, sa limite horizontale qui l'a fera réapparaitre à sa position de début
    public float positionDebut; //La position de début de l'objet, où elle réapparaitra après avoir atteint sa position de fin
    public float deplacementAleatoire; //Valeur déterminant la position aléatoire verticale possible pour l'objet

    //Fonction qui gère le déplacement à l'horizontale et le repositionnement aléatoire à la verticale
    void Update()
    {   
        //On applique la valeur de la variable vitesse et on fait déplacer l'objet
        transform.Translate(vitesse, 0, 0);

        //On détermine si la position horizontale a dépassé la position de fin 
        if(transform.position.x < positionFin)
        {
            //Alors, on crée une variable qui enregistrera une valeur aléatoire qui sera généré à partir de la valeur de la variable 'deplacementAleatoire'
            float valeurAleatoireY = Random.Range(-deplacementAleatoire, deplacementAleatoire);

            //On attribue des nouvelles coordonnées de position. On ramène l'objet à sa position de début (en X) et on la repositionne avec la variable de valeur aléatoire (en Y)
            transform.position = new Vector2(positionDebut, valeurAleatoireY);
        }
    }
}
