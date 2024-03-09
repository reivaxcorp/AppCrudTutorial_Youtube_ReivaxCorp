/*********************************************************************************
 * Nombre del Archivo:     ProgressText.cs
 * Descripción:            Clase que animará un texto cuando creamos un ítem.  
 *                         
 * Autor:                  Javier
 * Organización:           ReivaxCorp.
 *
 * Derechos de Autor © [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentación asociados (el “Software”), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluirán en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, específicamente en las carpetas “Assets/Scripts”.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D están sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, así como a las políticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA “TAL CUAL”, SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANTÍAS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROPÓSITO PARTICULAR Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SERÁN RESPONSABLES DE CUALQUIER RECLAMACIÓN, DAÑO U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCIÓN DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXIÓN CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
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
