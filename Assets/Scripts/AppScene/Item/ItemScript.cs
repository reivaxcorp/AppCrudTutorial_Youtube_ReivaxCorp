/*********************************************************************************
 * Nombre del Archivo:     ItemScrip.cs
 * Descripci�n:            Script encargado de habilitar algunos funciones para nuestro �tem 
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

using UnityEngine;

public class ItemScript : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        if (CheckItem())
        {
            boxCollider.enabled = false;
            rb.useGravity = false;
        }
    }

    /// <summary>
    /// habilitamos el box collider y la gravedad una vez que ya hayan sido ordenados
    /// desde el ManageItems.cs, ya que de otra menera
    /// colisionar�n unos con otros. 
    /// </summary>
    /// <returns></returns>
    public void EnablePhysicsItem()
    {
        if (!boxCollider.enabled && !rb.useGravity)
        {
            if (CheckItem())
            {
                boxCollider.enabled = true;
                rb.useGravity = true;
            }
        }
    }

    private bool CheckItem()
    {
        if (rb == null || boxCollider == null)
        {
            Debug.LogWarning("No hay un box collider � un Rigidbody en el �tem");
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("DeadZone"))
        {
            RestItem();
        }
    }

    /// <summary>
    /// Si se cae el �tem lo trasladamos a la posicion de nuestro ItemSceneConfig (Hierarchy)
    /// </summary>
    private void RestItem()
    {
        gameObject.transform.position = gameObject.transform.parent.position;
    }
}
