/*********************************************************************************
 * Nombre del Archivo:     ItemSceneConfig.cs 
 * Descripci�n:            Es una clase que nos ayudar� en colocar los �tems uno al lado del otro, 
 *                         evitando que se solapen, cada vez que hagamos las operaciones de a�adir o borrar
 *                         alg�n item, esta clase reordenara los �tems en la escena.
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor � [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentaci�n asociados (el �Software�), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, espec�ficamente en las carpetas �Assets/Scripts�.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D est�n sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, as� como a las pol�ticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA �TAL CUAL�, SIN GARANT�A DE NING�N TIPO, EXPRESA O IMPL�CITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANT�AS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SER�N RESPONSABLES DE CUALQUIER RECLAMACI�N, DA�O U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCI�N DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXI�N CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ItemSceneConfig : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] GameObject player;

    private List<GameObject> itemsGameObjects;

    private void Awake()
    {
        itemsGameObjects = new List<GameObject>();
    }

    public void SetItemGameObject(GameObject itemInScene)
    {
        itemsGameObjects.Add(itemInScene);
    } 

    /// <summary>
    /// Cuando leemos los datos de la base de datos local, debemos ordenar los �tems
    /// en la escena.
    /// </summary>
    public void OrderAllItemPositionInScene()
    {
        float nextPositionX = this.transform.position.x;

        if (item != null)
        {
            for (int index = 0; index < itemsGameObjects.Count; index++)
            {
                GameObject itemInScene = itemsGameObjects[index];

                if (itemInScene != null)
                {
                    SetItemParentToThis(itemInScene);
                    nextPositionX = TranslateItemSideBySide(itemInScene, nextPositionX);
                    EnablePhysicsItem(itemInScene);
                }
                else
                {
                    Debug.LogWarning("El �tem no existe enla escena!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Por favor coloca el item prefab (item) en el inspector");
        }
    }

    /// <summary>
    /// Cuando un usuario actualiza o a�ade un nuevo �tem, debemos ordenarlos en la escena
    /// Pero dejar los que no fueron modificados en su lugar. 
    /// </summary>
    /// <param name="itemsRemoteListUpdated"></param>
    public void OrderSomeItemPositionInScene(List<ItemRemote> itemsRemoteListUpdated)
    {
        if (player == null)
        {
            Debug.LogWarning("La referencia Player no existe en ItemSceneConfig en el inspector, por favor colocala");
            return;
        }

        List<GameObject> itemsToOrder = new List<GameObject>();

        for (int index = 0; index < itemsRemoteListUpdated.Count; index++)
        {
            GameObject itemInScene =
                itemsGameObjects.Find(item => item.name.Equals(itemsRemoteListUpdated[index].Id));

            if (itemInScene != null)
            {
                itemsToOrder.Add(itemInScene);
            }
        }

        if (item != null)
        {
            for (int index = 0; index < itemsToOrder.Count; index++)
            {
                GameObject itemInScene = itemsToOrder[index];

                if (itemInScene != null)
                {
                    SetItemParentToThis(itemInScene);
                    TranslateItemFromOfPlayer(itemInScene);
                    EnablePhysicsItem(itemInScene);
                }
                else
                {
                    Debug.LogWarning("El �tem no existe enla escena!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Por favor coloca el item prefab (item) en el inspector");
        }
    }

    /// <summary>
    /// Cuando eliminamos un �tem, tambi�n debemos eliminarlo de la lista de gameObjects.
    /// </summary>
    /// <param name="id"></param>
    public void DeleteOldGameObjectItem(string itemToDelete)
    {
        GameObject itemExists = itemsGameObjects.Find(item => item.name.Equals(itemToDelete));
        if (itemExists != null)
        {
            itemsGameObjects.Remove(itemExists);
        }
    }

    /// <summary>
    /// Traslada los �tems uno al lado del otro, cuando la aplicaci�n empieza.
    /// </summary>
    /// <param name="itemInScene">GameObject item</param>
    /// <param name="nextPositionX">La pr�xima posici�n</param>
    /// <returns></returns>
    private float TranslateItemSideBySide(GameObject itemInScene, float nextPositionX)
    {
        float nextPos = nextPositionX;

        Renderer rendeder = itemInScene.GetComponent<Renderer>();
        itemInScene.transform.position =
            new Vector3(nextPositionX, transform.position.y, transform.position.z);
        nextPos += rendeder.bounds.size.x * 2;
        return nextPos;
    }

   /// <summary>
   /// Trasladamos el item, reci�n actualizado � a�adido, al lado del jugador.
   /// </summary>
   /// <param name="itemInScene"></param>
    private void TranslateItemFromOfPlayer(GameObject itemInScene)
    {
        Renderer rendeder = itemInScene.GetComponent<Renderer>();
       
        Vector3 currentPlayerPosition =
            player.GetComponent<Transform>().position;
        // colocar los �tems actualizados delante del jugador
        itemInScene.transform.position =
            new Vector3(
                currentPlayerPosition.x,
                currentPlayerPosition.y + rendeder.bounds.size.y,
                currentPlayerPosition.z + rendeder.bounds.size.z * 2
            );
    }

    private void SetItemParentToThis(GameObject itemInScene)
    {
        itemInScene.transform.SetParent(this.transform);
    }

    private void EnablePhysicsItem(GameObject itemInScene)
    {
        ItemScript itemScript = itemInScene.GetComponent<ItemScript>();
        if (itemScript != null)
        {
            itemScript.EnablePhysicsItem();
        }
    }

}
