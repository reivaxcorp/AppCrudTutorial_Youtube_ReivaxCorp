/*********************************************************************************
 * Nombre del Archivo:     CheckUpdates.cs
 * Descripci�n:            Verificar� si hay algun cambio con los items remotos con los locales.
 *                         Enviara una lista del tipo ItemUpdate a nuestro ManageItems.cs, que contendra 
 *                         los �tems a actualizar,  � no. Si hay alg�n �tem nuevo que debamos agregar,
 *                         esta clase nos dar� su id y bajaremos los recursos necesarios
 *                         para crear ese �tem en nuestra aplicaci�n.  
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

public class CheckUpdates
{
    private static List<ItemRemote> listItemsChanged = new List<ItemRemote>();

    /// <summary>
    /// Verificamos los valores a actualizar y eliminar y aniadir, de la base de datos local
    /// </summary>
    /// <param name="itemsRemoteList">Lista de realtime database</param>
    /// <param name="itemsLocalList">Lista previamente guardada</param>
    /// <returns>
    /// Lista, con los items a actualizar, eliminar o crear.
    /// </returns>
    public static List<ItemUpdate> CheckUpdatesItems(
       List<ItemRemote> itemsRemoteList,
       List<ItemLocal> itemsLocalList
       )
    {
        listItemsChanged.Clear();

        List<ItemUpdate> itemUpdates = new List<ItemUpdate>();

        foreach (ItemLocal itemLocal in itemsLocalList)
        {

            ItemRemote itemRemote =
                itemsRemoteList.Find(p => p.Id.Equals(itemLocal.Id));

            // Se a removido un item de la base de datos
            if (itemRemote == null)
            {
                itemUpdates.Add(new ItemUpdate(
                    id: itemLocal.Id,
                    isImageUpdated: false,
                    isFieldsUpdated: false,
                    isRemove: true,
                    isAdd: false));
            }
            // Se ha cambiado los campos y la imag�n
            else if (IsFielsUpdate(itemRemote, itemLocal) && IsImageUpdated(itemRemote, itemLocal))
            {
                itemUpdates.Add(new ItemUpdate(
                  id: itemLocal.Id,
                  isImageUpdated: true,
                  isFieldsUpdated: true,
                  isRemove: false,
                  isAdd: false));

                listItemsChanged.Add(itemRemote);
            }
            // Solo se han cambiado los campos
            else if (IsFielsUpdate(itemRemote, itemLocal))
            {
                itemUpdates.Add(new ItemUpdate(
                 id: itemLocal.Id,
                 isImageUpdated: false,
                 isFieldsUpdated: true,
                 isRemove: false,
                 isAdd: false));

                listItemsChanged.Add(itemRemote);
            }
            // Se ha cambiado la imag�n
            else if (IsImageUpdated(itemRemote, itemLocal))
            {
                itemUpdates.Add(new ItemUpdate(
                id: itemLocal.Id,
                isImageUpdated: true,
                isFieldsUpdated: false,
                isRemove: false,
                isAdd: false));

                listItemsChanged.Add(itemRemote);
            }
            else
            {
                // sin cambios  el �tem como est�.
                itemUpdates.Add(new ItemUpdate(
                id: itemLocal.Id,
                isImageUpdated: false,
                isFieldsUpdated: false,
                isRemove: false,
                isAdd: false));
            }
        }

        // Nuevos items a a�adir
        foreach (ItemRemote itemRemote in itemsRemoteList)
        {
            ItemLocal itemLocal =
              itemsLocalList.Find(p => p.Id.Equals(itemRemote.Id));

            if (itemLocal == null)
            {
                itemUpdates.Add(new ItemUpdate(
                id: itemRemote.Id,
                isImageUpdated: false,
                isFieldsUpdated: false,
                isRemove: false,
                isAdd: true));

                listItemsChanged.Add(itemRemote);
            }
        }

        return itemUpdates;
    }

    /// <summary>
    /// Para obtener la lista que items se acabaron de actualizar, ignorando los que
    /// en realidad no han cambiado en nada.
    /// </summary>
    /// <returns></returns>
    public static List<ItemRemote> GetItemsChanged()
    {
        return listItemsChanged;
    }

    /// <summary>
    /// Si el producto cambio, necesitamos bajarlo de nuevo
    /// </summary>
    /// <param name="itemRemote"></param>
    /// <param name="itemLocal"></param>
    /// <returns></returns>
    private static bool IsFielsUpdate(ItemRemote itemRemote, ItemLocal itemLocal)
    {
        return !itemRemote.Name.Equals(itemLocal.Name);
    }

    // si cambia el el image_id_metadata, es probrable que cambie el nombre de la imagen, 
    // y si cambia a otra imagen cambia tambien el path.
    private static bool IsImageUpdated(ItemRemote itemRemote, ItemLocal itemLocal)
    {
        return !itemRemote.ImageName.Equals(itemLocal.ImageName);
    }

}
