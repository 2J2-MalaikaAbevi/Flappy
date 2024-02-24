using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Fonctionnement et utilit� g�n�rale du script:
   Gestion des d�placements horizontaux et des positions verticales des objets du jeu.
   Par : Mala�ka Abevi
   Derni�re modification : 23/02/2024
*/

public class DeplacementObjets : MonoBehaviour
{
    //D�clarations des variables (publiques)
    public float vitesse; //La vitesse de l'objet
    public float positionFin; //La position de fin de l'objet, sa limite horizontale qui l'a fera r�apparaitre � sa position de d�but
    public float positionDebut; //La position de d�but de l'objet, o� elle r�apparaitra apr�s avoir atteint sa position de fin
    public float deplacementAleatoire; //Valeur d�terminant la position al�atoire verticale possible pour l'objet

    //Fonction qui g�re le d�placement � l'horizontale et le repositionnement al�atoire � la verticale
    void Update()
    {   
        //On applique la valeur de la variable vitesse et on fait d�placer l'objet
        transform.Translate(vitesse, 0, 0);

        //On d�termine si la position horizontale a d�pass� la position de fin 
        if(transform.position.x < positionFin)
        {
            //Alors, on cr�e une variable qui enregistrera une valeur al�atoire qui sera g�n�r� � partir de la valeur de la variable 'deplacementAleatoire'
            float valeurAleatoireY = Random.Range(-deplacementAleatoire, deplacementAleatoire);

            //On attribue des nouvelles coordonn�es de position. On ram�ne l'objet � sa position de d�but (en X) et on la repositionne avec la variable de valeur al�atoire (en Y)
            transform.position = new Vector2(positionDebut, valeurAleatoireY);
        }
    }
}
