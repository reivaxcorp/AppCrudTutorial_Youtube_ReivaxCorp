/*********************************************************************************
 * Nombre del Archivo:     ProgressText.cs
 * Descripci�n:            Clase que animar� un texto cuando creamos un �tem.  
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor (c) [2024] ReivaxCorp
 * 
 * Permiso es otorgado, sin cargo, para que cualquier persona obtenga una copia
 * de este software y de los archivos de documentaci�n asociados (el "Software"),
 * para tratar en el Software sin restricci�n, incluyendo sin limitaci�n los
 * derechos para usar, copiar, modificar, fusionar, publicar, distribuir,
 * sublicenciar, y/o vender copias del Software, y para permitir a las personas a
 * quienes pertenezca el Software, sujeto a las siguientes condiciones:
 *
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en
 * todas las copias o partes sustanciales del Software.
 *
 * EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANT�A DE NING�N TIPO, EXPRESA O
 * IMPL�CITA, INCLUYENDO PERO NO LIMITADO A LAS GARANT�AS DE COMERCIABILIDAD,
 * IDONEIDAD PARA UN PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS
 * AUTORES O TITULARES DE DERECHOS DE AUTOR SER�N RESPONSABLES DE CUALQUIER
 * RECLAMACI�N, DA�O O OTRA RESPONSABILIDAD, YA SEA EN UNA ACCI�N DE CONTRATO, AGRAVIO
 * O DE OTRO MODO, DERIVADAS DE, FUERA DE O EN CONEXI�N CON EL SOFTWARE O EL USO U OTROS
 * TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using System.Collections;
using TMPro;
using UnityEngine;

public class ProgressText : MonoBehaviour
{
    private bool startAnimText;
    private Coroutine animationCoroutine; // Almacena la referencia a la corrutina

    public void StartProgressTextAnimation(string text, TextMeshProUGUI textToAnim)
    {
        this.startAnimText = true;
        textToAnim.color = Color.blue;
        animationCoroutine = StartCoroutine(AnimateText(text, textToAnim));
    }
    public void StopProgressTextAnimation()
    {
        this.startAnimText = false;
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private IEnumerator AnimateText(string text, TextMeshProUGUI textToAnim)
    {
        while (startAnimText) 
        {
            textToAnim.text = text + " .";
            yield return new WaitForSeconds(0.5f);

            textToAnim.text = text + " ..";
            yield return new WaitForSeconds(0.5f);

            textToAnim.text = text + " ...";
            yield return new WaitForSeconds(0.5f);

            textToAnim.text = text;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
