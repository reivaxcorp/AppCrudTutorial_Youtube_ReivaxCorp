/*********************************************************************************
 * Nombre del Archivo:     BullitScript.cs 
 * Descripci�n:            Clase que administrar� la bala � poder lanzada por el usuario, 
 *                         cuando toque alg�n item en la escena se abrir� el menu.
 *              
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
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private MenuManagerApp menuManagerApp;

    public void SetMenuManager(MenuManagerApp menuManagerApp)
    {
        this.menuManagerApp = menuManagerApp;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterSomeSeconds());
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (menuManagerApp != null)
            {
                menuManagerApp.ShowMenuUpdateItem(collision.gameObject.name);
            }
            else
            {
                Debug.LogWarning("MenuManagerApp, no se ha colocado desde PlayerShoot");
            }
        }
    }


    IEnumerator DestroyAfterSomeSeconds()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
